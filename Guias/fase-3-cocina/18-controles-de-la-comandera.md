# Guía 18 — Controles de la comandera

> **Estado:** completada

## Objetivo

Cuatro agregados sobre la comandera ya funcionando (guías 15 y 16), todos
orientados a que el cocinero se entere de los problemas **antes** de generar,
y a encontrar recetas por lo que hay en el depósito.

1. **Aviso de faltantes mientras se arma**, no recién al generar.
2. **Buscar recetas por ingrediente**, en la comandera y en el listado.
3. **Confirmación antes de generar**, porque descontar el stock no se deshace.
4. **Aviso de receta incompleta** (sin ingredientes o sin procedimiento).

Sin cambios en la base: todo se calcula con lo que ya está cargado.

## Decisiones

- **La revisión es una sola operación.** Faltantes y recetas incompletas se
  resuelven juntos en `RevisarAsync`, que se llama al pintar la pantalla y
  alimenta tanto el aviso del encabezado como el modal de confirmación. Es el
  mismo cálculo que ya corría dentro de `GenerarAsync`, ahora también visible
  antes de apretar el botón.
- **Los faltantes dejan de ser texto suelto.** `ResultadoGeneracion.Faltantes`
  pasa de `List<string>` a `List<FaltanteStock>`: la vista necesita el
  ingrediente, lo necesario y lo disponible por separado para armar la tabla, y
  de paso las cantidades se muestran con `FormatoCantidad` como en el resto del
  sistema (antes salían con `N2` crudo, "20,83 kg" contra "0,63 kg").
- **Faltar stock no bloquea armar la comanda, bloquea generarla.** Se puede
  seguir agregando recetas y cambiando comensales: el aviso se recalcula solo.
  El modal, cuando hay faltantes, no ofrece el botón de generar.
- **Receta incompleta avisa, no impide.** Una receta sin procedimiento se puede
  cocinar igual; el sistema lo marca y decide el cocinero. Aparece en el
  catálogo, en el listado de recetas y en el modal de confirmación.
- **Buscar por ingrediente, dos entradas distintas.** En la comandera el mismo
  buscador de siempre pasa a mirar también los ingredientes (buscar "lechuga"
  trae las recetas que la llevan) y aclara por qué apareció cada receta. En el
  listado de recetas, donde hay lugar, va un filtro propio de ingrediente.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/ComanderaViewModels.cs` | `FaltanteStock`, `RevisionComanda`, catálogo con conteos |
| `Services/IComandaService.cs` / `ComandaService.cs` | `RevisarAsync`, búsqueda por ingrediente en el catálogo |
| `Controllers/ComandasController.cs` | La revisión viaja a la vista; los faltantes se arman con su descripción |
| `Views/Comandas/Comandera.cshtml` | Aviso en el encabezado, modal de confirmación, avisos en el catálogo |
| `ViewModels/RecetaViewModels.cs` | `CantidadPasos` en el listado |
| `Services/IRecetaService.cs` / `RecetaService.cs` | Filtro por ingrediente |
| `Controllers/RecetasController.cs` | Combo de ingredientes |
| `Views/Recetas/Index.cshtml` | Filtro por ingrediente y aviso de receta incompleta |
| `wwwroot/css/site.css` | Aviso de faltantes y marca de receta incompleta |
| `Recetario-MVC.Tests/ComandaServiceTests.cs` | Revisión previa y búsqueda por ingrediente |

## Reglas

**Faltantes.** Se suma lo que pide cada receta del carrito escalada a los
comensales, con las modificaciones ya aplicadas, y se compara contra el stock.
Un mismo ingrediente usado por dos recetas se suma una sola vez: el problema es
el total, no cada receta por separado.

**Confirmación.** El modal resume cuántas recetas, cuántos comensales y qué
recetas son. Si hay faltantes muestra la tabla (ingrediente, lo que se necesita,
lo que hay) y el único botón es cerrar. Si no, avisa que se va a descontar el
stock y ofrece generar.

**Receta incompleta.** Sin ingredientes cargados o sin procedimiento cargado.
La primera es más grave —la comanda saldría sin nada que cocinar— pero las dos
se marcan igual, con el detalle en el texto de ayuda.

**Búsqueda por ingrediente.** El texto se compara contra nombre y código de la
receta y contra la descripción de sus ingredientes. Cuando una receta aparece
solo por el ingrediente, se muestra cuál fue, para que se entienda por qué está
en la lista.

## Verificación

- Con la comanda armada y comensales de más, el encabezado azul muestra el
  aviso rojo con la cantidad de ingredientes que no alcanzan; al bajar los
  comensales, desaparece solo.
- El botón de generar abre el modal: con faltantes muestra la tabla y no deja
  generar; sin faltantes, confirma y genera como siempre.
- Buscar "lechuga" en la comandera trae las recetas que la llevan, aclarando el
  ingrediente debajo del nombre.
- El filtro de ingrediente del listado de recetas devuelve las mismas recetas.
- Una receta sin procedimiento aparece marcada en el catálogo, en el listado y
  en el modal de confirmación.
- `dotnet test` en verde.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): avisar los problemas de la comanda antes de generarla
- Aviso de stock faltante mientras se arma el pedido
- Confirmación con el resumen de lo que se va a generar
- Marca de recetas sin ingredientes o sin procedimiento
- Búsqueda de recetas por ingrediente
```
