using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class ComandaService : IComandaService
{
    private readonly ApplicationDbContext _context;

    public ComandaService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>Ingrediente de una receta ya resuelto contra la base.</summary>
    private sealed record IngredienteBase(
        int IdIngrediente, string Nombre, decimal CantBruta, int IdUnidad, string Unidad);

    /// <summary>Ingrediente que realmente se cocina: receta escalada + modificaciones.</summary>
    private sealed record IngredienteEfectivo(
        int IdIngrediente, string Nombre, decimal Cantidad, int IdUnidad, string Unidad);

    // ================= Catálogo y previsualización =================

    public Task<List<RecetaCatalogoItem>> ListarCatalogoAsync(string? busqueda, int? idClasificacion)
    {
        var query = _context.Recetas.Where(r => r.Activo);

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(r => r.Nombre.Contains(busqueda) || r.Codigo.Contains(busqueda));

        if (idClasificacion.HasValue)
            query = query.Where(r => r.IdClasificacion == idClasificacion.Value);

        return query
            .OrderBy(r => r.Clasificacion.Nombre).ThenBy(r => r.Nombre)
            .Select(r => new RecetaCatalogoItem
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Clasificacion = r.Clasificacion.Nombre,
                PorcionesBase = r.PorcionesBase
            })
            .ToListAsync();
    }

    public async Task<IngredientesPreviewViewModel?> ObtenerIngredientesPreviewAsync(int idReceta, int comensales)
    {
        var receta = await _context.Recetas
            .Where(r => r.IdReceta == idReceta)
            .Select(r => new { r.Nombre, r.PorcionesBase })
            .FirstOrDefaultAsync();

        if (receta is null)
            return null;

        if (comensales <= 0)
            comensales = receta.PorcionesBase;

        var ingredientes = await _context.IngredientesReceta
            .Where(ir => ir.IdReceta == idReceta)
            .OrderBy(ir => ir.Ingrediente.Descripcion)
            .Select(ir => new
            {
                ir.IdIngrediente,
                ir.Ingrediente.Descripcion,
                ir.CantBruta,
                Unidad = ir.Unidad.Abreviatura,
                ir.Ingrediente.StockActual,
                ir.Ingrediente.StockMinimo
            })
            .ToListAsync();

        var factor = comensales / (decimal)receta.PorcionesBase;

        return new IngredientesPreviewViewModel
        {
            Receta = receta.Nombre,
            Comensales = comensales,
            Ingredientes = ingredientes.Select(i =>
            {
                var necesario = Math.Round(i.CantBruta * factor, 4);
                return new IngredientePreviewItem
                {
                    IdIngrediente = i.IdIngrediente,
                    Ingrediente = i.Descripcion,
                    Cantidad = necesario,
                    Unidad = i.Unidad,
                    CantidadTexto = FormatoCantidad.Formatear(necesario, i.Unidad),
                    Estado = StockEstado.De(i.StockActual, i.StockMinimo),
                    Alcanza = i.StockActual >= necesario
                };
            }).ToList()
        };
    }

    public async Task<CarritoItem?> ObtenerParaCarritoAsync(int idReceta)
    {
        return await _context.Recetas
            .Where(r => r.IdReceta == idReceta && r.Activo)
            .Select(r => new CarritoItem
            {
                IdReceta = r.IdReceta,
                Nombre = r.Nombre,
                Clasificacion = r.Clasificacion.Nombre,
                IdClasificacion = r.IdClasificacion,
                PorcionesBase = r.PorcionesBase
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<IngredienteEscaladoItem>> ListarIngredientesDeRecetaAsync(int idReceta)
    {
        return _context.IngredientesReceta
            .Where(ir => ir.IdReceta == idReceta)
            .OrderBy(ir => ir.Ingrediente.Descripcion)
            .Select(ir => new IngredienteEscaladoItem
            {
                IdIngrediente = ir.IdIngrediente,
                Ingrediente = ir.Ingrediente.Descripcion,
                Cantidad = ir.CantBruta,
                Unidad = ir.Unidad.Abreviatura
            })
            .ToListAsync();
    }

    public Task<List<Clasificacion>> ListarClasificacionesAsync() =>
        _context.Clasificaciones.OrderBy(c => c.Nombre).ToListAsync();

    public Task<List<Ingrediente>> ListarIngredientesAsync() =>
        _context.Ingredientes.Include(i => i.Unidad).OrderBy(i => i.Descripcion).ToListAsync();

    // ================= Responsables =================

    public async Task<Dictionary<int, string>> ResolverResponsablesAsync(CarritoComanda carrito)
    {
        var asignaciones = await ResolverPersonasAsync(carrito);
        return asignaciones.ToDictionary(a => a.Key, a => a.Value.Nombre + " " + a.Value.Apellido);
    }

    /// <summary>
    /// Un responsable por receta según su sector. Las recetas sin responsable
    /// propio (ej. decoraciones) heredan el de la primera que sí lo tenga,
    /// misma regla que el ucDashboardCocina original.
    /// </summary>
    private async Task<Dictionary<int, Persona>> ResolverPersonasAsync(CarritoComanda carrito)
    {
        var resultado = new Dictionary<int, Persona>();
        if (carrito.EstaVacio)
            return resultado;

        var sectores = carrito.Items.Select(i => i.IdClasificacion).Distinct().ToList();
        var personas = await _context.Personas
            .Where(p => p.IdClasificacion != null && sectores.Contains(p.IdClasificacion.Value))
            .OrderBy(p => p.IdPersona)
            .ToListAsync();

        var porSector = personas
            .GroupBy(p => p.IdClasificacion!.Value)
            .ToDictionary(g => g.Key, g => g.First());

        Persona? principal = null;
        foreach (var item in carrito.Items)
        {
            if (porSector.TryGetValue(item.IdClasificacion, out var propio))
            {
                resultado[item.IdReceta] = propio;
                principal ??= propio;
            }
        }

        if (principal is null)
            return resultado;

        foreach (var item in carrito.Items.Where(i => !resultado.ContainsKey(i.IdReceta)))
            resultado[item.IdReceta] = principal;

        return resultado;
    }

    // ================= Generación =================

    public async Task<ResultadoGeneracion> GenerarAsync(CarritoComanda carrito, string usuarioId)
    {
        var resultado = new ResultadoGeneracion();

        if (carrito.EstaVacio)
        {
            resultado.Error = "La comanda está vacía: agregá al menos una receta.";
            return resultado;
        }

        if (carrito.Comensales <= 0)
        {
            resultado.Error = "Ingresá una cantidad de comensales mayor a cero.";
            return resultado;
        }

        var responsables = await ResolverPersonasAsync(carrito);
        if (responsables.Count < carrito.Items.Count)
        {
            resultado.Error = "No hay responsable de cocina para el sector de alguna receta. " +
                              "Cargá responsables o sumá una receta con responsable asignado.";
            return resultado;
        }

        var ingredientesPorReceta = await CalcularEfectivosPorRecetaAsync(carrito);

        var faltantes = await ValidarStockAsync(ingredientesPorReceta);
        if (faltantes.Count > 0)
        {
            resultado.Faltantes = faltantes;
            return resultado;
        }

        var secciones = new List<SeccionComandaPdf>();
        var idsComandas = new List<int>();

        var estrategia = _context.Database.CreateExecutionStrategy();
        await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            foreach (var item in carrito.Items)
            {
                var efectivos = ingredientesPorReceta[item.IdReceta];

                var comanda = new Comanda
                {
                    IdReceta = item.IdReceta,
                    Fecha = DateOnly.FromDateTime(DateTime.Today),
                    Porciones = carrito.Comensales,
                    UsuarioId = usuarioId,
                    IdPersona = responsables[item.IdReceta].IdPersona
                };
                _context.Comandas.Add(comanda);
                await _context.SaveChangesAsync();

                await PersistirModificacionesAsync(comanda.IdComanda, item, carrito.Comensales);

                // Un movimiento de salida por ingrediente realmente usado:
                // la lista efectiva ya tiene aplicadas las modificaciones
                foreach (var ing in efectivos)
                {
                    _context.MovimientosStock.Add(new MovimientoStock
                    {
                        IdIngrediente = ing.IdIngrediente,
                        Tipo = TipoMovimiento.Salida,
                        Cantidad = ing.Cantidad,
                        IdUnidad = ing.IdUnidad,
                        Fecha = DateTime.Now,
                        UsuarioId = usuarioId,
                        Observaciones = $"Consumo comanda #{comanda.IdComanda}"
                    });

                    var ingrediente = await _context.Ingredientes.FindAsync(ing.IdIngrediente);
                    if (ingrediente is not null)
                        ingrediente.StockActual -= ing.Cantidad;
                }

                await _context.SaveChangesAsync();
                idsComandas.Add(comanda.IdComanda);

                secciones.Add(new SeccionComandaPdf
                {
                    Receta = item.Nombre,
                    Sector = item.Clasificacion,
                    Responsable = responsables[item.IdReceta].Nombre + " " + responsables[item.IdReceta].Apellido,
                    Ingredientes = efectivos
                        .Select(e => new IngredienteEscaladoItem
                        {
                            IdIngrediente = e.IdIngrediente,
                            Ingrediente = e.Nombre,
                            Cantidad = e.Cantidad,
                            Unidad = e.Unidad
                        })
                        .ToList(),
                    Modificaciones = item.Modificaciones.Select(m => m.Descripcion).ToList()
                });
            }

            await transaccion.CommitAsync();
        });

        await CompletarSeccionesAsync(secciones, carrito);
        resultado.IdsComandas = idsComandas;
        resultado.Secciones = secciones;
        return resultado;
    }

    /// <summary>Códigos y procedimiento de cada receta, para el PDF.</summary>
    private async Task CompletarSeccionesAsync(List<SeccionComandaPdf> secciones, CarritoComanda carrito)
    {
        var ids = carrito.Items.Select(i => i.IdReceta).ToList();

        var codigos = await _context.Recetas
            .Where(r => ids.Contains(r.IdReceta))
            .ToDictionaryAsync(r => r.IdReceta, r => r.Codigo);

        var pasos = await _context.Procedimientos
            .Where(p => ids.Contains(p.IdReceta))
            .OrderBy(p => p.NroPaso)
            .Select(p => new { p.IdReceta, p.NroPaso, p.Descripcion })
            .ToListAsync();

        for (var i = 0; i < secciones.Count; i++)
        {
            var idReceta = carrito.Items[i].IdReceta;
            secciones[i].Codigo = codigos.GetValueOrDefault(idReceta, string.Empty);
            secciones[i].Pasos = pasos
                .Where(p => p.IdReceta == idReceta)
                .Select(p => new PasoItem { NroPaso = p.NroPaso, Descripcion = p.Descripcion })
                .ToList();
        }
    }

    private async Task PersistirModificacionesAsync(int idComanda, CarritoItem item, int comensales)
    {
        if (item.Modificaciones.Count == 0)
            return;

        var baseIngredientes = await ObtenerIngredientesBaseAsync(item.IdReceta);
        var factor = comensales / (decimal)item.PorcionesBase;

        foreach (var mod in item.Modificaciones)
        {
            var original = mod.IdIngredienteOriginal is int idO
                ? baseIngredientes.FirstOrDefault(b => b.IdIngrediente == idO)
                : null;

            var cantidad = ResolverCantidad(mod, original, factor);
            if (cantidad <= 0)
                continue;

            var idUnidad = mod.IdIngredienteReemplazo is int idR
                ? (await _context.Ingredientes.FindAsync(idR))?.IdUnidad ?? original?.IdUnidad
                : original?.IdUnidad;

            if (idUnidad is null)
                continue;

            _context.Modificaciones.Add(new Modificacion
            {
                IdComanda = idComanda,
                Tipo = mod.Tipo,
                IdIngredienteOriginal = mod.IdIngredienteOriginal,
                IdIngredienteReemplazo = mod.IdIngredienteReemplazo,
                Cantidad = cantidad,
                IdUnidad = idUnidad.Value
            });
        }
    }

    /// <summary>
    /// Cantidad de una modificación: la explícita si el cocinero la ingresó
    /// (adición, o sustitución entre unidades distintas) o, si no, la que el
    /// ingrediente original tiene en la receta ya escalada.
    /// </summary>
    private static decimal ResolverCantidad(ModificacionCarrito mod, IngredienteBase? original, decimal factor)
    {
        if (mod.Cantidad is decimal explicita)
            return Math.Round(explicita, 4);

        return original is null ? 0m : Math.Round(original.CantBruta * factor, 4);
    }

    private Task<List<IngredienteBase>> ObtenerIngredientesBaseAsync(int idReceta)
    {
        return _context.IngredientesReceta
            .Where(ir => ir.IdReceta == idReceta)
            .Select(ir => new IngredienteBase(
                ir.IdIngrediente,
                ir.Ingrediente.Descripcion,
                ir.CantBruta,
                ir.IdUnidad,
                ir.Unidad.Abreviatura))
            .ToListAsync();
    }

    /// <summary>
    /// Lo que realmente se cocina por receta: la receta escalada a los
    /// comensales con las modificaciones aplicadas. Es a la vez lo que se
    /// descuenta de stock y lo que sale impreso en la comanda.
    /// </summary>
    private async Task<Dictionary<int, List<IngredienteEfectivo>>> CalcularEfectivosPorRecetaAsync(
        CarritoComanda carrito)
    {
        var resultado = new Dictionary<int, List<IngredienteEfectivo>>();

        // Ingredientes que aparecen solo por modificaciones (reemplazos y agregados)
        var idsExtra = carrito.Items
            .SelectMany(i => i.Modificaciones)
            .Where(m => m.IdIngredienteReemplazo.HasValue)
            .Select(m => m.IdIngredienteReemplazo!.Value)
            .Distinct()
            .ToList();

        var extras = await _context.Ingredientes
            .Where(i => idsExtra.Contains(i.IdIngrediente))
            .Select(i => new { i.IdIngrediente, i.Descripcion, i.IdUnidad, Unidad = i.Unidad.Abreviatura })
            .ToDictionaryAsync(i => i.IdIngrediente);

        foreach (var item in carrito.Items)
        {
            var baseIngredientes = await ObtenerIngredientesBaseAsync(item.IdReceta);
            var factor = carrito.Comensales / (decimal)item.PorcionesBase;
            var efectivos = new List<IngredienteEfectivo>();

            foreach (var ing in baseIngredientes)
            {
                var cantidad = Math.Round(ing.CantBruta * factor, 4);
                var mod = item.Modificaciones.FirstOrDefault(m =>
                    m.IdIngredienteOriginal == ing.IdIngrediente &&
                    m.Tipo != TipoModificacion.Adicion);

                if (mod is null)
                {
                    efectivos.Add(new IngredienteEfectivo(
                        ing.IdIngrediente, ing.Nombre, cantidad, ing.IdUnidad, ing.Unidad));
                    continue;
                }

                // Eliminación: el ingrediente no se usa ni se descuenta.
                // Sustitución: en su lugar entra el reemplazo, con la misma cantidad
                // salvo que el cocinero haya indicado otra.
                if (mod.Tipo == TipoModificacion.Sustitucion &&
                    mod.IdIngredienteReemplazo is int idR &&
                    extras.TryGetValue(idR, out var reemplazo))
                {
                    efectivos.Add(new IngredienteEfectivo(
                        reemplazo.IdIngrediente,
                        reemplazo.Descripcion,
                        mod.Cantidad is decimal c ? Math.Round(c, 4) : cantidad,
                        reemplazo.IdUnidad,
                        reemplazo.Unidad));
                }
            }

            foreach (var mod in item.Modificaciones.Where(m => m.Tipo == TipoModificacion.Adicion))
            {
                if (mod.IdIngredienteReemplazo is int idR &&
                    extras.TryGetValue(idR, out var agregado) &&
                    mod.Cantidad is decimal cantidad && cantidad > 0)
                {
                    efectivos.Add(new IngredienteEfectivo(
                        agregado.IdIngrediente,
                        agregado.Descripcion,
                        Math.Round(cantidad, 4),
                        agregado.IdUnidad,
                        agregado.Unidad));
                }
            }

            resultado[item.IdReceta] = efectivos;
        }

        return resultado;
    }

    /// <summary>Suma lo requerido por todas las recetas y lo compara con el stock.</summary>
    private async Task<List<string>> ValidarStockAsync(
        Dictionary<int, List<IngredienteEfectivo>> ingredientesPorReceta)
    {
        var requerido = ingredientesPorReceta.Values
            .SelectMany(lista => lista)
            .GroupBy(i => i.IdIngrediente)
            .ToDictionary(g => g.Key, g => new
            {
                Nombre = g.First().Nombre,
                Unidad = g.First().Unidad,
                Cantidad = g.Sum(x => x.Cantidad)
            });

        var ids = requerido.Keys.ToList();
        var stocks = await _context.Ingredientes
            .Where(i => ids.Contains(i.IdIngrediente))
            .ToDictionaryAsync(i => i.IdIngrediente, i => i.StockActual);

        var faltantes = new List<string>();
        foreach (var (id, datos) in requerido)
        {
            var disponible = stocks.GetValueOrDefault(id, 0m);
            if (disponible < datos.Cantidad)
            {
                faltantes.Add(
                    $"{datos.Nombre}: se necesitan {datos.Cantidad:N2} {datos.Unidad} " +
                    $"y hay {disponible:N2} {datos.Unidad}.");
            }
        }

        return faltantes;
    }

    // ================= Consulta =================

    public Task<List<ComandaListaItem>> ListarAsync(DateOnly? desde, DateOnly? hasta, int? idReceta)
    {
        var query = _context.Comandas.AsQueryable();

        if (desde.HasValue)
            query = query.Where(c => c.Fecha >= desde.Value);
        if (hasta.HasValue)
            query = query.Where(c => c.Fecha <= hasta.Value);
        if (idReceta.HasValue)
            query = query.Where(c => c.IdReceta == idReceta.Value);

        return query
            .OrderByDescending(c => c.Fecha)
            .ThenByDescending(c => c.IdComanda)
            .Select(c => new ComandaListaItem
            {
                IdComanda = c.IdComanda,
                Fecha = c.Fecha,
                Receta = c.Receta.Nombre,
                Clasificacion = c.Receta.Clasificacion.Nombre,
                Porciones = c.Porciones,
                Responsable = c.Persona.Nombre + " " + c.Persona.Apellido,
                Usuario = c.Usuario.Nombre + " " + c.Usuario.Apellido,
                CantidadModificaciones = c.Modificaciones.Count
            })
            .ToListAsync();
    }

    public async Task<ComandaDetalleViewModel?> ObtenerDetalleAsync(int idComanda)
    {
        var comanda = await _context.Comandas
            .Where(c => c.IdComanda == idComanda)
            .Select(c => new ComandaDetalleViewModel
            {
                IdComanda = c.IdComanda,
                Fecha = c.Fecha,
                IdReceta = c.IdReceta,
                Receta = c.Receta.Nombre,
                Codigo = c.Receta.Codigo,
                Clasificacion = c.Receta.Clasificacion.Nombre,
                Porciones = c.Porciones,
                PorcionesBase = c.Receta.PorcionesBase,
                Responsable = c.Persona.Nombre + " " + c.Persona.Apellido,
                Usuario = c.Usuario.Nombre + " " + c.Usuario.Apellido
            })
            .FirstOrDefaultAsync();

        if (comanda is null)
            return null;

        var factor = comanda.Porciones / (decimal)comanda.PorcionesBase;
        comanda.Ingredientes = (await _context.IngredientesReceta
                .Where(ir => ir.IdReceta == comanda.IdReceta)
                .OrderBy(ir => ir.Ingrediente.Descripcion)
                .Select(ir => new
                {
                    ir.IdIngrediente,
                    ir.Ingrediente.Descripcion,
                    ir.CantBruta,
                    Unidad = ir.Unidad.Abreviatura
                })
                .ToListAsync())
            .Select(x => new IngredienteEscaladoItem
            {
                IdIngrediente = x.IdIngrediente,
                Ingrediente = x.Descripcion,
                Cantidad = Math.Round(x.CantBruta * factor, 4),
                Unidad = x.Unidad
            })
            .ToList();

        comanda.Pasos = await _context.Procedimientos
            .Where(p => p.IdReceta == comanda.IdReceta)
            .OrderBy(p => p.NroPaso)
            .Select(p => new PasoItem
            {
                IdProcedimiento = p.IdProcedimiento,
                NroPaso = p.NroPaso,
                Descripcion = p.Descripcion
            })
            .ToListAsync();

        comanda.Modificaciones = await _context.Modificaciones
            .Where(m => m.IdComanda == idComanda)
            .OrderBy(m => m.IdModificacion)
            .Select(m => new ModificacionItem
            {
                Tipo = m.Tipo,
                IngredienteOriginal = m.IngredienteOriginal!.Descripcion,
                IngredienteReemplazo = m.IngredienteReemplazo!.Descripcion,
                Cantidad = m.Cantidad,
                Unidad = m.Unidad.Abreviatura
            })
            .ToListAsync();

        return comanda;
    }
}
