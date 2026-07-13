# Guía 07 — Ingredientes

> **Estado:** completada

## Objetivo

Primer ABM completo de la app web: ingredientes con listado + búsqueda, alta/edición con código autogenerado y baja protegida. Define el **molde de módulo** que van a repetir Proveedores, Recetas y los demás: servicio con interfaz, ViewModels por vista, controller delgado con `[Authorize(Roles)]`, vistas con el diseño de la guía 01, e ítem de sidebar habilitado.

## Qué se mantiene y qué mejora respecto del WinForms (`ucIngredientes`)

| Aspecto | WinForms | Web |
|---|---|---|
| Código `ING###` | Autogenerado, no editable | Igual (se calcula al entrar al alta) |
| Listado | Grilla completa | Tabla con **búsqueda por código/descripción** y **badges de estado de stock** (crítico/bajo/normal, umbrales de la guía 06) |
| Baja | Eliminación directa | **Baja protegida**: si el ingrediente está referenciado (recetas, movimientos, modificaciones, costos) se bloquea con mensaje claro. Sus precios sí se borran en cascada |
| Stock | Editable en el form | Alta con stock inicial; en edición se puede ajustar **por ahora** — cuando exista el módulo de movimientos (guía 12) pasará a ser de solo lectura y auditado |
| Precios por proveedor | Pestaña propia | Se agregan en la **guía 08** (necesitan el ABM de proveedores) |

## Decisión transversal: cultura es-AR

Los decimales del dominio (stock, precios, cantidades) se cargan **con coma** (`12,5`):

- `Program.cs` fija `RequestLocalization` y cultura por defecto `es-AR` (el model binding acepta coma y los `ToString` formatean con coma).
- `site.js` sobreescribe los métodos `number`/`range` de jQuery Validation para aceptar coma del lado cliente.

Aplica a todos los módulos que vienen, no solo a este.

## Piezas

| Archivo | Contenido |
|---|---|
| `Helpers/StockEstado.cs` | Umbral y cálculo de estado de stock compartido (lo usa también el dashboard) |
| `Services/IIngredienteService.cs` / `IngredienteService.cs` | Listar con búsqueda, obtener, generar código, crear, editar, eliminar con pre-chequeo de referencias, listar unidades |
| `ViewModels/IngredienteViewModels.cs` | `IngredienteFormViewModel` (con validaciones) y `IngredienteListaItem` |
| `Controllers/IngredientesController.cs` | Solo Admin: `Index(busqueda)`, `Crear`, `Editar`, `Eliminar` (POST) |
| `Views/Ingredientes/Index.cshtml` | Búsqueda + tabla + modal de confirmación de borrado |
| `Views/Ingredientes/Crear.cshtml` / `Editar.cshtml` | Formulario compartido vía parcial `_Form` |
| `Views/Shared/_Layout.cshtml` | Se habilita el ítem Ingredientes de la sidebar |
| `Program.cs` | Registro del servicio + cultura es-AR |

## Verificación

- Crear un ingrediente → código `ING001` autogenerado y de solo lectura; con stock `2,5` y mínimo `10` aparece con badge **Crítico** en el listado y en el dashboard.
- Crear un segundo → `ING002`.
- Buscar por texto parcial filtra la tabla.
- Editar descripción/stock y guardar.
- Eliminar un ingrediente sin referencias → desaparece con alerta de éxito (los referenciados se prueban en la guía 09 cuando existan recetas).
- El código duplicado o descripción vacía muestran validación en español.
- El rol Cocina no ve el ítem en la sidebar y `/Ingredientes` le da acceso denegado.

## Próximo paso

[Guía 08 — Proveedores](08-proveedores.md) (incluye precios por ingrediente).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar ABM de ingredientes
- Listado con búsqueda y badges de estado de stock
- Código ING autogenerado y baja protegida por referencias
- Cultura es-AR con decimales con coma en binding y validación
```
