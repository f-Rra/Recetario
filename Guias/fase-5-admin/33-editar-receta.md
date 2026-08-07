# Guía 33 — Editar receta y alta en modal

> **Estado:** completada

## Objetivo

Séptimo commit del rediseño de Admin. La pantalla más cargada del sistema
—datos, ingredientes con rendimiento y procedimiento— se reordena, y el alta de
receta pasa a abrirse en modal desde el listado.

## Decisiones de diseño (cerradas con maquetas)

- **Los datos, en una barra horizontal** arriba: código, nombre, clasificación,
  porciones base y activa, con Guardar al final. Ocupaban una columna entera al
  costado para cinco campos que se tocan poco.
- **Ingredientes y procedimiento lado a lado**, cada uno con su **fila de carga
  al pie**, separada por una línea: donde termina la lista es donde se sigue
  cargando.
- Los pasos usan los **círculos numerados** del detalle de comanda, en vez de
  una lista `<ol>`.
- Los títulos de panel llevan el conteo — "INGREDIENTES · 9" — y los vacíos
  explican la consecuencia: sin ingredientes no se puede costear ni armar una
  comanda; sin pasos, la comanda sale sin instrucciones.
- Arriba, accesos directos a **Ver como cocina** y **Costear**, que antes
  obligaban a volver al listado.

## El alta de receta es la excepción

Ingredientes, proveedores, responsables y usuarios se crean y se terminan en el
modal. **Una receta no**: crearla es solo el primer paso, porque lo que
realmente hay que cargar son los ingredientes y el procedimiento. El modal pide
los datos básicos y al guardar sigue a la edición, que es donde está el trabajo.

## Cuidado: los mensajes de validación necesitan el ViewModel

El primer intento dejó el listado con `@@model List<RecetaListaItem>` y metió el
formulario del modal con `name="Nueva.X"` a mano. Los errores del servidor
**no se mostraban**: `asp-validation-for` los lee del ModelState, y sin una
propiedad del modelo a la que apuntar no hay dónde escribirlos; los `span`
escritos a mano solo los llena la validación del navegador.

Por eso el listado pasó a `RecetasPaginaViewModel`, con la lista, el formulario
del alta y el modal a reabrir — el mismo molde que las otras pantallas de Admin.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/RecetaViewModels.cs` | `RecetasPaginaViewModel` |
| `Controllers/RecetasController.cs` | `ArmarPaginaAsync`; el alta bindea con prefijo y se va su GET |
| `Views/Recetas/Index.cshtml` | Modelo nuevo y modal de alta |
| `Views/Recetas/Editar.cshtml` | Reescritura: barra de datos y dos columnas |
| `Views/Recetas/Crear.cshtml` | **Eliminada** |
| `wwwroot/css/site.css` | Lenguaje `rec-*` |

## Verificación

- La barra de datos entra en una sola línea y guarda los cambios.
- Ingredientes y procedimiento quedan lado a lado, con su fila de carga al pie.
- Agregar un ingrediente calcula la bruta; agregar pasos los numera solos y
  quitar uno del medio renumera el resto.
- El alta abre en modal: vacía vuelve con los tres errores y el modal abierto;
  completa lleva a la edición de la receta nueva.
- La ruta vieja (`Crear` GET) devuelve 404.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): reordenar la edición de recetas y crear en modal
- Los datos pasan a una barra y el contenido a dos columnas
- Cada panel carga al pie, donde termina su lista
- El alta abre en modal y sigue a la edición
```
