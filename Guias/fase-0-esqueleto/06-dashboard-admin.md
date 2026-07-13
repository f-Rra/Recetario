# Guía 06 — Dashboard Admin

> **Estado:** completada

## Objetivo

Reemplazar el placeholder de Home por el Dashboard Admin del diseño (guía 01): cards de métricas y resumen de ingredientes con stock bajo. Cierra la **fase 0**: a partir de acá, cada módulo nuevo (fases 1–5) va llenando estas métricas con datos reales.

Introduce además el **patrón de servicios** que van a usar todas las guías siguientes: interfaz + implementación en `Services/`, registrados en DI, con los controllers delgados.

## Decisiones

- **Métricas** (las 4 del mockup del artifact): recetas activas, total de ingredientes, ingredientes en stock crítico (en rojo), costo promedio por porción (promedio del último costeo de cada receta).
- **Estados de stock** (mejora sobre `vw_StockCritico`, que solo distinguía crítico):
  - **Crítico**: `StockActual < StockMinimo` (misma semántica que la vista vieja)
  - **Bajo**: `StockActual < StockMinimo × 1.25` (margen del 25% para anticiparse)
  - **Normal**: el resto
- La tabla del dashboard lista hasta 8 ingredientes bajos/críticos ordenados por gravedad; con la base vacía muestra un estado vacío amigable.
- **Dashboard por rol**: el rol Cocina ve por ahora una bienvenida simple; su dashboard real (comandas del día) es la guía 11.
- Con la base recién sembrada todas las métricas dan 0: correcto, se llenan con las fases 1–2.

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/IDashboardService.cs` | Contrato: `ObtenerResumenAsync()` |
| `Services/DashboardService.cs` | Consultas LINQ de métricas y stock bajo |
| `ViewModels/DashboardViewModel.cs` | Métricas + lista de `IngredienteStockItem` (con estado calculado) |
| `Controllers/HomeController.cs` | Inyecta el servicio; Admin → dashboard, Cocina → bienvenida |
| `Views/Home/Index.cshtml` | Cards de métricas + tabla de stock bajo |
| `Views/Home/Cocina.cshtml` | Bienvenida temporal del rol Cocina |
| `Program.cs` | `AddScoped<IDashboardService, DashboardService>()` |

## Verificación

- Login admin → 4 cards en 0 y estado vacío en la tabla (base sin datos).
- Insertando ingredientes de prueba por SQL (uno crítico, uno normal), el dashboard cuenta y lista solo el crítico con su badge rojo.
- Login cocina → bienvenida simple, sin métricas de costos.

## Próximo paso

Fase 1 — [Guía 07: Ingredientes](../fase-1-crud/07-ingredientes.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar dashboard admin con métricas y stock bajo
- DashboardService con patrón servicio + DI para las guías siguientes
- Cards de métricas y tabla de stock con estados crítico/bajo/normal
- Bienvenida temporal para el rol Cocina
```
