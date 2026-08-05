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

    /// <summary>Ingrediente que entra por una modificación, no por la receta.</summary>
    private sealed record IngredienteExterno(
        int IdIngrediente, string Descripcion, int IdUnidad, string Unidad);

    // ================= Catálogo y previsualización =================

    public async Task<List<RecetaCatalogoItem>> ListarCatalogoAsync(string? busqueda, int? idClasificacion)
    {
        var texto = string.IsNullOrWhiteSpace(busqueda) ? null : busqueda.Trim();
        var query = _context.Recetas.Where(r => r.Activo);

        // El buscador mira también los ingredientes: "lechuga" trae las recetas
        // que la llevan, útil para darle salida a lo que está por vencerse
        if (texto is not null)
            query = query.Where(r =>
                r.Nombre.Contains(texto) ||
                r.Codigo.Contains(texto) ||
                r.Ingredientes.Any(ir => ir.Ingrediente.Descripcion.Contains(texto)));

        if (idClasificacion.HasValue)
            query = query.Where(r => r.IdClasificacion == idClasificacion.Value);

        var catalogo = await query
            .OrderBy(r => r.Clasificacion.Nombre).ThenBy(r => r.Nombre)
            .Select(r => new RecetaCatalogoItem
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Clasificacion = r.Clasificacion.Nombre,
                IdClasificacion = r.IdClasificacion,
                PorcionesBase = r.PorcionesBase,
                CantidadIngredientes = r.Ingredientes.Count,
                CantidadPasos = r.Procedimientos.Count
            })
            .ToListAsync();

        if (texto is not null)
            await CoincidenciaPorIngrediente.ResolverAsync(_context, catalogo, texto);

        return catalogo;
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
                // La misma cantidad que se va a descontar al generar
                var necesario = RedondeoCocina.Redondear(i.CantBruta * factor, i.Unidad);
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
        var reemplazos = await ObtenerIngredientesDeModificacionesAsync(item);

        foreach (var mod in item.Modificaciones)
        {
            var original = mod.IdIngredienteOriginal is int idO
                ? baseIngredientes.FirstOrDefault(b => b.IdIngrediente == idO)
                : null;

            var reemplazo = mod.IdIngredienteReemplazo is int idR && reemplazos.TryGetValue(idR, out var r)
                ? r
                : null;

            // La modificación se guarda en la unidad del ingrediente que entra
            var idUnidad = reemplazo?.IdUnidad ?? original?.IdUnidad;
            if (idUnidad is null)
                continue;

            var cantidad = ResolverCantidad(mod, original, factor, reemplazo?.Unidad);
            if (cantidad <= 0)
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
    /// (adición) o, si no, la que el ingrediente original tiene en la receta
    /// escalada, convertida a la unidad del reemplazo si hace falta.
    /// </summary>
    private static decimal ResolverCantidad(
        ModificacionCarrito mod, IngredienteBase? original, decimal factor, string? unidadDestino)
    {
        if (mod.Cantidad is decimal explicita)
            return RedondeoCocina.Redondear(explicita, unidadDestino ?? original?.Unidad ?? string.Empty);

        if (original is null)
            return 0m;

        var escalada = original.CantBruta * factor;

        var enDestino = unidadDestino is null
            ? escalada
            : Unidades.Convertir(escalada, original.Unidad, unidadDestino) ?? escalada;

        // La modificación guarda lo mismo que se descontó del stock
        return RedondeoCocina.Redondear(enDestino, unidadDestino ?? original.Unidad);
    }

    /// <summary>Ingredientes que entran por una modificación (reemplazos y agregados).</summary>
    private async Task<Dictionary<int, IngredienteExterno>> ObtenerIngredientesDeModificacionesAsync(
        params CarritoItem[] items)
    {
        var ids = items
            .SelectMany(i => i.Modificaciones)
            .Where(m => m.IdIngredienteReemplazo.HasValue)
            .Select(m => m.IdIngredienteReemplazo!.Value)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
            return new Dictionary<int, IngredienteExterno>();

        return await _context.Ingredientes
            .Where(i => ids.Contains(i.IdIngrediente))
            .Select(i => new IngredienteExterno(
                i.IdIngrediente, i.Descripcion, i.IdUnidad, i.Unidad.Abreviatura))
            .ToDictionaryAsync(i => i.IdIngrediente);
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
        var extras = await ObtenerIngredientesDeModificacionesAsync(carrito.Items.ToArray());

        foreach (var item in carrito.Items)
        {
            var baseIngredientes = await ObtenerIngredientesBaseAsync(item.IdReceta);
            var factor = carrito.Comensales / (decimal)item.PorcionesBase;
            var efectivos = new List<IngredienteEfectivo>();

            foreach (var ing in baseIngredientes)
            {
                // Lo que sale del depósito: redondeado a una medida usable
                var cantidad = RedondeoCocina.Redondear(ing.CantBruta * factor, ing.Unidad);
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
                // Sustitución: en su lugar entra el reemplazo con la misma cantidad,
                // convertida si se mide en otra unidad de la misma familia.
                if (mod.Tipo == TipoModificacion.Sustitucion &&
                    mod.IdIngredienteReemplazo is int idR &&
                    extras.TryGetValue(idR, out var reemplazo))
                {
                    var cantidadReemplazo = RedondeoCocina.Redondear(
                        mod.Cantidad ?? Unidades.Convertir(cantidad, ing.Unidad, reemplazo.Unidad) ?? cantidad,
                        reemplazo.Unidad);

                    efectivos.Add(new IngredienteEfectivo(
                        reemplazo.IdIngrediente,
                        reemplazo.Descripcion,
                        cantidadReemplazo,
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
                        RedondeoCocina.Redondear(cantidad, agregado.Unidad),
                        agregado.IdUnidad,
                        agregado.Unidad));
                }
            }

            resultado[item.IdReceta] = efectivos;
        }

        return resultado;
    }

    // ================= Revisión previa =================

    /// <summary>
    /// Lo mismo que revisa <see cref="GenerarAsync"/>, pero sin generar nada:
    /// así el cocinero se entera de los problemas mientras arma el pedido y no
    /// después de apretar el botón.
    /// </summary>
    public async Task<RevisionComanda> RevisarAsync(CarritoComanda carrito)
    {
        var revision = new RevisionComanda();
        if (carrito.EstaVacio || carrito.Comensales <= 0)
            return revision;

        var ingredientesPorReceta = await CalcularEfectivosPorRecetaAsync(carrito);
        revision.Faltantes = await ValidarStockAsync(ingredientesPorReceta);
        revision.Incompletas = await BuscarIncompletasAsync(carrito);
        return revision;
    }

    /// <summary>
    /// Recetas del carrito a las que les falta contenido. No bloquean: una
    /// receta sin procedimiento se cocina igual, pero conviene saberlo antes
    /// de imprimir la comanda.
    /// </summary>
    private async Task<List<string>> BuscarIncompletasAsync(CarritoComanda carrito)
    {
        var ids = carrito.Items.Select(i => i.IdReceta).ToList();

        var recetas = await _context.Recetas
            .Where(r => ids.Contains(r.IdReceta))
            .Select(r => new
            {
                r.Nombre,
                Ingredientes = r.Ingredientes.Count,
                Pasos = r.Procedimientos.Count
            })
            .ToListAsync();

        return recetas
            .Where(r => r.Ingredientes == 0 || r.Pasos == 0)
            .Select(r => (r.Ingredientes, r.Pasos) switch
            {
                (0, 0) => $"{r.Nombre} no tiene ingredientes ni procedimiento cargados.",
                (0, _) => $"{r.Nombre} no tiene ingredientes cargados.",
                _ => $"{r.Nombre} no tiene procedimiento cargado."
            })
            .ToList();
    }

    /// <summary>Suma lo requerido por todas las recetas y lo compara con el stock.</summary>
    private async Task<List<FaltanteStock>> ValidarStockAsync(
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

        var faltantes = new List<FaltanteStock>();
        foreach (var (id, datos) in requerido)
        {
            var disponible = stocks.GetValueOrDefault(id, 0m);
            if (disponible < datos.Cantidad)
            {
                faltantes.Add(new FaltanteStock
                {
                    IdIngrediente = id,
                    Ingrediente = datos.Nombre,
                    Necesario = datos.Cantidad,
                    Disponible = disponible,
                    Unidad = datos.Unidad
                });
            }
        }

        return faltantes.OrderBy(f => f.Ingrediente).ToList();
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
                CantidadModificaciones = c.Modificaciones.Count,
                // Con una sola modificación la columna muestra el texto en vez
                // del conteo, así no hay que abrir el detalle para saber cuál fue
                PrimeraModificacion = c.Modificaciones
                    .OrderBy(m => m.IdModificacion)
                    .Select(m => new ModificacionItem
                    {
                        Tipo = m.Tipo,
                        IngredienteOriginal = m.IngredienteOriginal!.Descripcion,
                        IngredienteReemplazo = m.IngredienteReemplazo!.Descripcion,
                        Cantidad = m.Cantidad,
                        Unidad = m.Unidad.Abreviatura
                    })
                    .FirstOrDefault()
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
                // Se reescala desde la receta: hay que redondear igual que al
                // generar, o el detalle mostraría otros números que el PDF
                Cantidad = RedondeoCocina.Redondear(x.CantBruta * factor, x.Unidad),
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
