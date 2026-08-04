# Guía 19 — Rediseño: la comandera

> **Estado:** completada

## Objetivo

Primera pantalla del rediseño de cocina acordado sobre maquetas (agosto 2026):
la base "carrito" con la comanda como **ticket**. Cambia solo la presentación:
las funciones de las guías 15 y 18 quedan intactas (carrito en sesión,
previsualización escalada, modificaciones, revisión de faltantes, confirmación,
PDF).

El rediseño completo se commitea **de a una pantalla**: esta guía es la
comandera; siguen historial (20), detalle (21), recetas (22) y stock (23).

## Decisiones de diseño (cerradas con maquetas)

- **Dos columnas**: catálogo a la izquierda, la comanda como **ticket** fijo a
  la derecha (fondo blanco, separadores de guiones, título espaciado).
- **Catálogo como tarjetas**: nombre de la receta **en azul**, debajo un punto
  de color por sector + clasificación + **cantidad de ingredientes** (ya no la
  base). El botón **+** agrega directo; si ya está, muestra ✓ deshabilitado.
- **Ingredientes al tocar la tarjeta**: la fila se expande y lista los
  ingredientes en **una columna con puntos de guía**, escalados a los
  comensales. Los que no alcanzan van en rojo (sin negrita) con ⚠. Desaparece
  el panel lateral de ingredientes y los badges de estado de stock.
- **Buscador simple** ("Buscar…") en la misma fila que las clasificaciones como
  píldoras. Busca en nombre e ingredientes (guía 18) y aclara "lleva X" bajo el
  nombre. Ya no hay select de clasificación ni selección múltiple con botón
  Agregar.
- **Ticket**: nombre en azul sin cantidad por renglón (los comensales son
  globales), línea gris con sector · responsable, íconos ✎ y ✕ **en azul**,
  modificaciones como etiquetas celestes con su ✕. **Comensales en negrita**
  con contador − / + que aplica al soltar. Los **faltantes como renglones
  rojos** dentro del ticket (reemplazan a la pastilla del encabezado). Botón
  verde Generar y enlace al historial al pie.
- La receta incompleta se marca en la meta de su tarjeta ("⚠ sin
  procedimiento"), en ámbar.
- Los modales de **modificación** y de **confirmación** quedan como están.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/ComanderaViewModels.cs` | `IdClasificacion` en el ítem de catálogo (para el color del punto) |
| `Services/ComandaService.cs` | El catálogo trae `IdClasificacion` |
| `Views/Comandas/Comandera.cshtml` | Reescritura completa de la vista |
| `wwwroot/css/site.css` | Lenguaje nuevo (`cmd-*`) reemplaza al tablero de la guía 15 |

Sin cambios en controller, servicios (más allá de la proyección) ni tests de
lógica: `dotnet test` debe seguir en verde sin tocar nada.

## Verificación

- El catálogo lista las recetas como tarjetas con el punto de color y la
  cantidad de ingredientes; tocar una la expande con los ingredientes en una
  columna y cierra la anterior.
- El + agrega la receta al ticket; el contador de comensales actualiza y
  recalcula los faltantes, que aparecen como renglones rojos en el ticket.
- Buscar por ingrediente muestra "lleva X"; las píldoras filtran por sector.
- ✎ abre el modal de modificación; ✕ quita la receta; el ✕ de una etiqueta
  quita esa modificación.
- Generar abre la confirmación de siempre (bloqueada si hay faltantes) y
  descarga el PDF.
- `dotnet test` en verde.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md.

```
feat(mvc): rediseñar la comandera como catálogo y ticket
- Catálogo de tarjetas con ingredientes expandibles en columna
- La comanda como ticket con los faltantes adentro
- Buscador simple junto a las clasificaciones como píldoras
```
