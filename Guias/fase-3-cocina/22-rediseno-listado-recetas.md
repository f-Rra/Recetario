# Guía 22 — Rediseño: listado de recetas

> **Estado:** pendiente

## Objetivo

Cuarta pantalla del rediseño (maquetas de agosto 2026): el listado como
**grilla de fichas**. La vista es compartida con Admin, así que las acciones de
administración se conservan dentro de la ficha.

## Decisiones de diseño (cerradas con maquetas)

- **Buscador simple** ("Buscar…") en la misma fila que las clasificaciones como
  píldoras — mismo control que la comandera (guía 19). Desaparecen el select de
  clasificación y el de ingrediente: la búsqueda por ingrediente va por texto,
  con la aclaración "lleva X" en la ficha (requiere pasar la búsqueda de
  recetas por el mismo camino que el catálogo de la comandera).
- **Fichas en grilla de tres**: ceja del sector en mayúsculas azules, nombre
  grande, meta con ingredientes y pasos, cinta ámbar "sin procedimiento" para
  las incompletas.
- **Cocina** ve el botón Ver receta; **Admin** además Costear, Editar y
  Eliminar como botones chicos en la ficha (mismo modal de eliminación).

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Recetas/Index.cshtml` | Reescritura como grilla de fichas |
| `Services/RecetaService.cs` | Búsqueda por texto que incluye ingredientes + "lleva X" |
| `ViewModels/RecetaViewModels.cs` | `IngredienteCoincidente` en el ítem de lista |
| `Controllers/RecetasController.cs` | Se quita el combo de ingredientes del listado |
| `wwwroot/css/site.css` | Estilos de fichas (`fch-*`) |
| `Recetario-MVC.Tests/` | La búsqueda por ingrediente en el listado |

## Verificación

- Las fichas muestran sector, nombre, meta y la cinta de incompleta.
- Buscar "lechuga" trae las recetas que la llevan con la aclaración.
- Como Admin aparecen las acciones y el modal de eliminar sigue andando.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar el listado de recetas como fichas
- Grilla de tres con ceja de sector y cinta de incompleta
- Búsqueda única por nombre o ingrediente con aclaración
```
