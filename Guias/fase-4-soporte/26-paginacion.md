# Guía 26 — Paginación

> **Estado:** completada

## Objetivo

Hoy el sistema tiene 6 recetas y 27 ingredientes: todo entra en una pantalla.
Cuando se cargue el catálogo real, los listados que crecen sin techo van a
traer cientos de filas en cada request. Esta guía define **dónde** hace falta
paginar, **con qué mecanismo** en cada caso y **qué no se toca**.

Se escribe ahora para que quede decidido; se implementa cuando el volumen lo
pida o antes de poner el sistema en producción.

## El criterio

No todo listado necesita un paginador. La pregunta es qué lo hace crecer:

- **Crece con los datos cargados** (recetas, ingredientes, proveedores) →
  paginar.
- **Crece con el uso, para siempre** (comandas, movimientos de stock) →
  paginar sí o sí: son los que más rápido se vuelven inmanejables.
- **Está acotado por la organización** (responsables, usuarios) → no paginar.
  Son decenas como máximo y un paginador ahí es ruido.
- **Se opera, no se lee** (el catálogo de la comandera) → ver más abajo: acá
  paginar molesta más de lo que ayuda.

## Dónde sí

| Pantalla | Hoy | Orden estable |
|---|---|---|
| Historial de movimientos | corta en 100 sin avisar cuántos hay | fecha desc, id desc |
| Historial de comandas | trae todo el rango de fechas | fecha desc, id desc |
| Listado de recetas | trae todas | nombre |
| Ingredientes (Admin) | trae todos | descripción |
| Proveedores (Admin) | trae todos | nombre |

**El más urgente es el historial de movimientos**: hoy `StockService` hace
`Take(100)` y la vista aclara "se muestran los últimos 100". Eso no es un
tope de rendimiento, es información que se pierde sin manera de llegar a ella.
La paginación lo reemplaza.

## Dónde no

- **Responsables y Usuarios**: acotados por la gente que trabaja en el lugar.
- **Stock de ingredientes**: son dos columnas simultáneas (bodega y cámara);
  un paginador que avance las dos a la vez es confuso y uno por columna, peor.
  Ya tiene búsqueda y filtro por estado, que es como se usa de verdad —
  "mostrame los críticos", no "mostrame la página 3". Si con el catálogo real
  queda largo, se revisa.
- **Detalle de receta y de comanda**: los ingredientes de una receta son
  finitos por naturaleza.
- **Catálogo de la comandera**: paginar mientras armás un pedido es peor que
  scrollear —perdés de vista lo que ya elegiste—. La solución ahí es el
  buscador y las píldoras de clasificación, que ya están. Si el catálogo se
  vuelve enorme, la alternativa es cargar más al llegar al final, no páginas.

## Decisiones

- **Paginación en el servidor.** `Skip`/`Take` en la consulta, no cortar en
  memoria: si no, el problema que se quiere resolver sigue estando.
- **Un tipo compartido**, `Paginado<T>`, con la página actual, el tamaño, el
  total de filas y los ítems. Los servicios devuelven eso en vez de `List<T>`.
- **Un partial compartido** para el paginador, así las cinco pantallas se ven
  y se comportan igual, con el estilo de píldoras del rediseño.
- **Los filtros viajan en los enlaces.** Pasar de página no puede perder la
  búsqueda, la clasificación ni el rango de fechas.
- **El orden tiene que ser total.** Si dos filas empatan, el desempate por id
  evita que una fila aparezca en dos páginas o en ninguna. Los listados de hoy
  ya ordenan así, salvo donde haya que agregar el id.
- **Tamaño de página según la pantalla**: las tablas densas (movimientos,
  comandas) van más filas por página que la grilla de fichas de recetas.
- **El total se muestra siempre**, como el chip que ya tienen los historiales:
  "128 movimientos" dice más que "página 1 de 7".
- Los **separadores de día** de los historiales siguen funcionando dentro de
  cada página, aunque un día quede partido entre dos.

## Dos listados con dos usos

Ingredientes y recetas se piden de dos formas distintas, así que cada servicio
expone las dos y el nombre dice cuál es cuál:

- `ListarAsync` trae **todo** lo que coincide. Lo necesitan el stock —que
  muestra los dos depósitos a la vez—, los combos de ingrediente y el selector
  de recetas del costeo.
- `ListarPaginadoAsync` trae **una página**. Lo usan sus propios listados.

En ingredientes, además, la página viaja en cada enlace y en cada formulario:
editar algo de la página 2 tiene que devolverte a la página 2, no al principio.

## Tamaños

| Pantalla | Por página | Por qué |
|---|---|---|
| Movimientos · Comandas | 25 | tablas densas |
| Ingredientes | 25 | la lista del maestro-detalle es angosta |
| Recetas · Proveedores | 12 | grillas de fichas, tres por fila |

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/Paginado.cs` | `Paginado<T>` y `PaginadorViewModel` |
| `Services/Paginacion.cs` | El `Skip`/`Take` sobre la consulta |
| `Views/Shared/_Paginador.cshtml` | El control, con los filtros en los enlaces |
| `Services/StockService.cs` | Historial paginado; se va el `Take(100)` |
| `Services/ComandaService.cs` | Historial de comandas paginado |
| `Services/RecetaService.cs` · `IngredienteService.cs` · `ProveedorService.cs` | Listados paginados |
| Controllers y vistas de esas cinco pantallas | Parámetro de página |
| `wwwroot/css/site.css` | Estilo del paginador, ancho y angosto |
| `Recetario-MVC.Tests/PaginacionTests.cs` | Cortes, bordes y filtros |

## Verificación

Contra los datos que ya están cargados:

- **239 movimientos** en 10 páginas; la última trae 14 filas y el tope de 100
  desaparece.
- **27 ingredientes** en 2 páginas: 25 y 2. Elegir uno de la segunda no vuelve
  a la primera.
- Juntar las dos páginas devuelve las 30 filas de la prueba sin repetir ni
  saltear ninguna.
- Una página fuera de rango cae en la última que existe.
- Cambiar de página conserva el filtro de tipo, fechas y búsqueda.
- Recetas y proveedores no muestran paginador: entran en una página.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): paginar los listados que crecen con el uso
- Historiales de comandas y movimientos, recetas, ingredientes y proveedores
- Paginador compartido que conserva los filtros
```
