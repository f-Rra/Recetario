# Guía 25 — Ajustes y las dos pantallas que faltaban

> **Estado:** completada

## Objetivo

Cerrar el rediseño de cocina: dos retoques sobre la comandera y las dos
pantallas que quedaron con el estilo viejo — el **detalle de receta** y el
**historial de movimientos de stock**.

## Ajustes de la comandera

- El **+ del catálogo** se apoyaba abajo del círculo: la línea de texto lo
  bajaba. Pasa a ser el ícono `bi-plus-lg` centrado por grilla (y el ✓ de
  "ya está en la comanda", `bi-check-lg`), como el resto del sistema.
- **"Comensales"** queda en negrita pero en gris: es una etiqueta, no un dato.
- **"(hay 0,91 u)"** también en gris. El rojo se reserva para el problema —
  lo que falta —; lo que hay es contexto.

## Detalle de receta

Adopta el lenguaje del detalle de comanda (guía 21):

- Cabecera como tarjeta: nombre, código y clasificación, las **porciones base**
  como número grande y, para Admin, Costear y Editar como píldoras.
- Dos columnas: **INGREDIENTES** en una columna con puntos de guía y
  **PROCEDIMIENTO** con los pasos numerados en círculos celestes.
- Las cantidades siguen siendo las de la receta, **sin redondear** (guía 24):
  acá se ve la definición, no una comanda.

## Historial de movimientos

Adopta la tabla azul del historial de comandas (guía 20):

- Chip con la cantidad de movimientos del filtro; filtros como píldoras y
  botón Filtrar azul sólido.
- Encabezado azul pleno y **separadores de día**, con la hora al lado del
  ingrediente: la fecha deja de repetirse en cada fila.
- El ingrediente va en negro, no en azul: acá la fila no lleva a ningún lado.
- Los tipos siguen con sus badges (entrada verde, salida roja, ajuste celeste).

## Se va la barra de título

La barra blanca de arriba repetía lo que ya dice el menú lateral y se comía
unos 50 px de alto — en la comandera, media receta más a la vista.

- El `<header class="topbar">` desaparece del layout. El `<title>` del
  navegador sigue usando `ViewData["Title"]`.
- Las pantallas que **se presentan solas** marcan `ViewData["SinTitulo"]`:
  comandera, historial de comandas, detalle de comanda, listado y detalle de
  receta, y stock. Cada una ya tiene su propia entrada — el ticket, el chip con
  el total, la cabecera con el nombre, los títulos de bodega y cámara.
- Las demás (el historial de movimientos y todas las de Admin) reciben el
  encabezado dentro del contenido, sobre el fondo gris y sin barra. Cuando se
  rediseñe Admin, cada una se irá presentando sola y la marca se irá cayendo.

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Shared/_Layout.cshtml` | Se va la topbar; el título pasa al contenido |
| `Views/Comandas/Comandera.cshtml` | Íconos en el botón de agregar, sin título |
| `Views/Comandas/Index.cshtml` · `Detalle.cshtml` | Sin título |
| `Views/Recetas/Index.cshtml` | Sin título |
| `Views/Recetas/Detalle.cshtml` | Reescritura con cabecera y dos columnas |
| `Views/Stock/Index.cshtml` | Sin título |
| `Views/Stock/Historial.cshtml` | Reescritura con la tabla azul |
| `wwwroot/css/site.css` | Centrado del botón, grises, `hst-ingrediente`, `titulo-pantalla` |

## Verificación

- El + y el ✓ quedan centrados en el círculo.
- "Comensales" y "(hay …)" se leen en gris.
- El detalle de una receta muestra ingredientes y procedimiento en dos
  columnas, con las cantidades de la receta sin redondear.
- El historial de movimientos agrupa por día y filtra por ingrediente, tipo y
  fechas.
- Ninguna pantalla de cocina muestra la barra blanca; las de Admin conservan su
  encabezado, ahora sobre el fondo.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): completar el rediseño de cocina
- Detalle de receta e historial de movimientos con el estilo nuevo
- Sin barra de título: cada pantalla se presenta sola
- Botón de agregar centrado y avisos en gris
```
