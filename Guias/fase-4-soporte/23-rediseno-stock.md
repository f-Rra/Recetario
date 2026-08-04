# Guía 23 — Rediseño: stock de ingredientes

> **Estado:** completada

## Objetivo

Quinta y última pantalla del rediseño (maquetas de agosto 2026): los dos
depósitos **lado a lado**, sin pestañas. La vista es compartida con Admin, que
conserva sus acciones (movimientos, inventario).

## Decisiones de diseño (cerradas con maquetas)

- **Bodega y cámara en dos columnas simultáneas**, cada una con su título en
  mayúsculas azules y el conteo de ingredientes. La búsqueda filtra en ambas.
- **Filtro por estado**: píldoras Todos / Bajos / Críticos que dejan solo los
  ingredientes en ese estado, en los dos depósitos.
- **Filas tarjeta**: nombre en negrita, la etiqueta **Crítico** o **Bajo**
  pegada al nombre — los que están bien no llevan nada — y la **cantidad
  alineada a la derecha** con `FormatoCantidad`.
- Admin: el botón de movimiento por fila y "Hacer inventario" por depósito se
  conservan con el estilo nuevo; el enlace al historial de movimientos sigue
  arriba.

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Stock/Index.cshtml` | Reescritura a dos columnas con filtro de estado |
| `Controllers/StockController.cs` | Parámetro de estado en el índice (si se filtra en servidor) |
| `wwwroot/css/site.css` | Estilos de las columnas de depósito (`stk-*`) |

## Verificación

- Los dos depósitos se ven a la vez con sus conteos; la búsqueda filtra ambos.
- "Críticos" deja solo los críticos de bodega y cámara; "Todos" restaura.
- Solo los bajos y críticos llevan etiqueta; las cantidades quedan alineadas.
- Las acciones de Admin (movimiento, inventario) siguen andando.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar el stock con los dos depósitos a la vista
- Bodega y cámara lado a lado con filtro por estado
- Etiquetas solo para bajos y críticos, cantidades alineadas
```
