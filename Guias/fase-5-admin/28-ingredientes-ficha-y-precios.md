# Guía 28 — Ingredientes: la ficha con los precios adentro

> **Estado:** completada

## Objetivo

Segundo commit del rediseño de Admin. Ingredientes pasa a ser **una sola
pantalla**: la lista a la izquierda y, a la derecha, todo lo del elegido —
datos, stock, precios y en qué recetas se usa.

Hoy son cuatro pantallas: el listado, el alta, la edición y el historial de
precios. Quedan una.

## Decisiones de diseño (cerradas con maquetas)

- **Maestro-detalle**: lista angosta a la izquierda, ficha a la derecha. La
  selección **viaja en la URL** (`?id=`), así que el enlace se puede compartir
  y el navegador recuerda dónde estabas. Sin id, se elige el primero; si el id
  no está en la lista filtrada, también.
- **Se absorbe la pantalla de precios**. El historial y el alta de precio viven
  en la ficha, que es donde se los busca. La ruta `Precios` desaparece.
- **Alta y edición como modales**, sin cambiar de página ni perder el filtro.
  Si el formulario vuelve con errores, la pantalla se rearma y el modal
  **se reabre solo** con los mensajes puestos.
- **Se suma "se usa en"**: las recetas que llevan el ingrediente. Es lo que
  explica por qué a veces no se puede eliminar.
- La ficha muestra el stock con `FormatoCantidad`, igual que la lista: 1256 ml
  se leen "1,26 L" en los dos lados. El **mínimo** pasó a la línea de datos,
  que es donde se consulta.

## Cuidado: dos formularios en la misma página

El alta y la edición usan el mismo `IngredienteFormViewModel`, así que sin
prefijo **comparten los nombres de campo**: un error en el alta se pintaba
también en el modal de edición. La edición se bindea con
`[Bind(Prefix = "Datos")]` y su partial se renderiza con ese prefijo. Los
precios ya venían con el suyo (`NuevoPrecio`).

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/IngredienteViewModels.cs` | `IngredientesPaginaViewModel`, `IngredienteDetalleViewModel` |
| `Services/IIngredienteService.cs` / `IngredienteService.cs` | `ObtenerDetalleAsync` |
| `Controllers/IngredientesController.cs` | Todo vuelve al Index; se van `Crear`, `Editar` y `Precios` GET |
| `Views/Ingredientes/Index.cshtml` | La pantalla entera con sus cuatro modales |
| `Views/Ingredientes/_Form.cshtml` | Sirve al alta y a la edición |
| `Views/Ingredientes/Crear.cshtml` · `Editar.cshtml` · `Precios.cshtml` | **Eliminadas** |
| `wwwroot/css/site.css` | Lenguaje `mdt-*`, que el costeo va a reutilizar |
| `Recetario-MVC.Tests/IngredienteServiceTests.cs` | La ficha |

## Verificación

- La lista trae los 27 ingredientes; tocar uno cambia la ficha y la URL.
- Buscar filtra la lista y la ficha cae en el primer resultado.
- El alta abre en modal; con un campo vacío vuelve con el error y el modal
  abierto, **sin manchar** el formulario de edición.
- Editar cambia el mínimo y la ficha lo refleja.
- Cargar y quitar precios actualiza el panel; el más reciente queda "Vigente".
- Eliminar un ingrediente con movimientos sigue siendo rechazado con su aviso.
- Las rutas viejas (`Crear`, `Editar/1`, `Precios/1`) devuelven 404.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): unificar ingredientes en una ficha con sus precios
- Lista y detalle en una pantalla, con la selección en la URL
- El historial de precios deja de ser una pantalla aparte
- Alta y edición en modales que se reabren si hay errores
```
