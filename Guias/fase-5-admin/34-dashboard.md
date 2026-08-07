# Guía 34 — Dashboard: números arriba, actividad abajo

> **Estado:** completada

## Objetivo

Octavo y último commit del rediseño de Admin. El dashboard combina las dos
opciones elegidas: el **panel de números** arriba y el **registro de actividad**
abajo.

## Las cuatro tarjetas

Se quedan **Recetas activas** e **Ingredientes**, que dicen qué hay cargado.
Las otras dos se reemplazaron para no repetir el mismo tipo de dato:

| Antes | Ahora | Por qué |
|---|---|---|
| Stock crítico | **Porciones del mes** | El volumen real de producción, con la comparación contra el mes anterior. Lo crítico ya se ve en el panel de stock bajo, abajo. |
| Costo promedio por porción | **Valor del inventario** | Un promedio de promedios no significaba gran cosa; la plata parada en el depósito sí. |

Cada tarjeta suma contexto al pie: cuántas recetas están incompletas, cómo se
reparten los ingredientes entre bodega y cámara, la variación contra el mes
pasado, y cuántos ingredientes entraron en la valorización.

### Dos precisiones que cambian el nombre de las cosas

- **Son porciones, no comensales.** Una comanda es por receta: un servicio de
  tres recetas para 200 personas son 600 porciones, no 600 comensales. La
  tarjeta dice lo que realmente suma.
- **El inventario se valoriza con lo que tiene precio.** Un ingrediente sin
  precio no se puede valorizar, así que el pie aclara cuántos entraron —
  "23 de 27 con precio"— en vez de dar un total que parezca completo.

El valor usa el **precio vigente** con la misma semántica del costeo (guía 10):
fecha más reciente, desempate por id.

## El registro de actividad

Se arma con lo que el sistema **audita de verdad**: comandas generadas y
movimientos de stock. Precios y ediciones de recetas no guardan cuándo se
hicieron, así que no aparecen — inventarlos habría sido peor que omitirlos.

Dos decisiones:

- **Los consumos de una comanda no se listan.** Generar una comanda de nueve
  ingredientes crea nueve movimientos; mostrarlos taparía todo lo demás. La
  comanda ya está en el feed, y sus movimientos siguen en el historial de stock.
- **Las comandas no tienen hora**, solo fecha: dentro de cada día van primero y
  después los movimientos, esos sí con su hora.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/DashboardViewModel.cs` | Las métricas nuevas, `ComandaRecienteItem`, `ActividadItem` |
| `Services/DashboardService.cs` | Valorización, porciones por mes y armado del feed |
| `Views/Home/Index.cshtml` | Tarjetas, dos paneles y línea de tiempo |
| `wwwroot/css/site.css` | Lenguaje `dsh-*` |
| `Recetario-MVC.Tests/DashboardServiceTests.cs` | Valorización, porciones y feed |

## Verificación

Contrastado contra la base con SQL:

- Porciones de agosto: **135**; de julio: **1751** → variación **−92,3%**.
- Valor del inventario: **$ 4.445.798,19**, con los 27 ingredientes valorizados.
- El feed trae 12 entradas agrupadas por día, sin ningún "Consumo comanda".
- Stock bajo y últimas comandas coinciden con las otras pantallas.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar el dashboard con métricas nuevas y actividad
- Porciones del mes y valor del inventario reemplazan a las dos tarjetas viejas
- Registro cronológico de comandas y movimientos de stock
```
