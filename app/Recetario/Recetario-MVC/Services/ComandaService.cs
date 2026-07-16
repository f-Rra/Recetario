using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
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

    public Task<List<ComandaListaItem>> ListarPorFechaAsync(DateOnly fecha)
    {
        return _context.Comandas
            .Where(c => c.Fecha == fecha)
            .OrderByDescending(c => c.IdComanda)
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

        // Ingredientes escalados a las porciones de la comanda (ex vw_Comanda + sp_AjustarReceta)
        var factor = comanda.Porciones / (decimal)comanda.PorcionesBase;
        comanda.Ingredientes = (await _context.IngredientesReceta
                .Where(ir => ir.IdReceta == comanda.IdReceta)
                .OrderBy(ir => ir.Ingrediente.Descripcion)
                .Select(ir => new
                {
                    ir.Ingrediente.Descripcion,
                    ir.CantBruta,
                    Unidad = ir.Unidad.Abreviatura
                })
                .ToListAsync())
            .Select(x => new IngredienteEscaladoItem
            {
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

        comanda.NuevaModificacion.IdComanda = idComanda;
        return comanda;
    }

    public async Task<PanelCocinaViewModel> ObtenerPanelDelDiaAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var comandas = await ListarPorFechaAsync(hoy);

        return new PanelCocinaViewModel
        {
            ComandasHoy = comandas.Count,
            PorcionesHoy = comandas.Sum(c => c.Porciones),
            Comandas = comandas
        };
    }

    public Task<List<Receta>> ListarRecetasActivasAsync() =>
        _context.Recetas.Where(r => r.Activo).OrderBy(r => r.Nombre).ToListAsync();

    public Task<List<Persona>> ListarResponsablesAsync() =>
        _context.Personas.Include(p => p.Clasificacion)
            .OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToListAsync();

    public Task<List<Ingrediente>> ListarIngredientesAsync() =>
        _context.Ingredientes.Include(i => i.Unidad).OrderBy(i => i.Descripcion).ToListAsync();

    public async Task<(int? IdComanda, string? Error)> RegistrarAsync(
        int idReceta, int porciones, string usuarioId, int? idPersona)
    {
        if (porciones <= 0)
            return (null, "Las porciones deben ser mayores a cero.");

        var receta = await _context.Recetas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.IdReceta == idReceta);

        if (receta is null || !receta.Activo)
            return (null, "La receta no existe o está inactiva.");

        // Regla del SP viejo: sin persona indicada, asigna un responsable
        // del sector (la clasificación de la receta)
        if (idPersona is null)
        {
            idPersona = await _context.Personas
                .Where(p => p.IdClasificacion == receta.IdClasificacion)
                .OrderBy(p => p.IdPersona)
                .Select(p => (int?)p.IdPersona)
                .FirstOrDefaultAsync();

            if (idPersona is null)
                return (null, "No hay responsable de cocina para el sector de esta receta. Cargalo en Responsables.");
        }
        else if (!await _context.Personas.AnyAsync(p => p.IdPersona == idPersona))
        {
            return (null, "El responsable indicado no existe.");
        }

        // Transacción: comanda + movimientos de salida + stock, todo o nada.
        // Reemplaza a trg_ConsumirStockComanda.
        var estrategia = _context.Database.CreateExecutionStrategy();
        int idComanda = 0;

        await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            var comanda = new Comanda
            {
                IdReceta = idReceta,
                Fecha = DateOnly.FromDateTime(DateTime.Today),
                Porciones = porciones,
                UsuarioId = usuarioId,
                IdPersona = idPersona.Value
            };
            _context.Comandas.Add(comanda);
            await _context.SaveChangesAsync();

            var ingredientesReceta = await _context.IngredientesReceta
                .Where(ir => ir.IdReceta == idReceta)
                .Include(ir => ir.Ingrediente)
                .ToListAsync();

            foreach (var ir in ingredientesReceta)
            {
                var consumo = Math.Round(ir.CantBruta / receta.PorcionesBase * porciones, 4);

                _context.MovimientosStock.Add(new MovimientoStock
                {
                    IdIngrediente = ir.IdIngrediente,
                    Tipo = TipoMovimiento.Salida,
                    Cantidad = consumo,
                    IdUnidad = ir.IdUnidad,
                    Fecha = DateTime.Now,
                    UsuarioId = usuarioId,
                    Observaciones = $"Consumo comanda #{comanda.IdComanda}"
                });

                ir.Ingrediente.StockActual -= consumo;
            }

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();
            idComanda = comanda.IdComanda;
        });

        return (idComanda, null);
    }

    public async Task<string?> AgregarModificacionAsync(ModificacionFormViewModel modelo, string usuarioId)
    {
        var comanda = await _context.Comandas.FindAsync(modelo.IdComanda);
        if (comanda is null)
            return "La comanda no existe.";

        var tipo = modelo.Tipo!.Value;
        var cantidad = Math.Round(modelo.Cantidad!.Value, 4);

        // Validación por tipo (el schema viejo permitía combinaciones sin sentido)
        var errorTipo = tipo switch
        {
            TipoModificacion.Sustitucion when modelo.IdIngredienteOriginal is null || modelo.IdIngredienteReemplazo is null
                => "La sustitución necesita ingrediente original y reemplazo.",
            TipoModificacion.Adicion when modelo.IdIngredienteReemplazo is null || modelo.IdIngredienteOriginal is not null
                => "La adición lleva solo el ingrediente agregado (campo reemplazo/agregado).",
            TipoModificacion.Eliminacion when modelo.IdIngredienteOriginal is null || modelo.IdIngredienteReemplazo is not null
                => "La eliminación lleva solo el ingrediente original.",
            _ => null
        };
        if (errorTipo is not null)
            return errorTipo;

        var original = modelo.IdIngredienteOriginal is int idO
            ? await _context.Ingredientes.FindAsync(idO)
            : null;
        var reemplazo = modelo.IdIngredienteReemplazo is int idR
            ? await _context.Ingredientes.FindAsync(idR)
            : null;

        if (modelo.IdIngredienteOriginal is not null && original is null)
            return "El ingrediente original no existe.";
        if (modelo.IdIngredienteReemplazo is not null && reemplazo is null)
            return "El ingrediente de reemplazo no existe.";

        // La unidad de la modificación es la del ingrediente que entra en juego
        var idUnidad = (reemplazo ?? original)!.IdUnidad;

        var estrategia = _context.Database.CreateExecutionStrategy();
        await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            _context.Modificaciones.Add(new Modificacion
            {
                IdComanda = modelo.IdComanda,
                Tipo = tipo,
                IdIngredienteOriginal = modelo.IdIngredienteOriginal,
                IdIngredienteReemplazo = modelo.IdIngredienteReemplazo,
                Cantidad = cantidad,
                IdUnidad = idUnidad
            });

            // Mejora sobre el trigger viejo: los ajustes de stock quedan auditados
            if (original is not null)
            {
                _context.MovimientosStock.Add(new MovimientoStock
                {
                    IdIngrediente = original.IdIngrediente,
                    Tipo = TipoMovimiento.Entrada,
                    Cantidad = cantidad,
                    IdUnidad = original.IdUnidad,
                    Fecha = DateTime.Now,
                    UsuarioId = usuarioId,
                    Observaciones = $"Devolución por modificación de comanda #{modelo.IdComanda}"
                });
                original.StockActual += cantidad;
            }

            if (reemplazo is not null)
            {
                _context.MovimientosStock.Add(new MovimientoStock
                {
                    IdIngrediente = reemplazo.IdIngrediente,
                    Tipo = TipoMovimiento.Salida,
                    Cantidad = cantidad,
                    IdUnidad = reemplazo.IdUnidad,
                    Fecha = DateTime.Now,
                    UsuarioId = usuarioId,
                    Observaciones = $"Consumo por modificación de comanda #{modelo.IdComanda}"
                });
                reemplazo.StockActual -= cantidad;
            }

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();
        });

        return null;
    }
}
