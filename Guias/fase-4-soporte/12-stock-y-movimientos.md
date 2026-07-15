# Guía 12 — Stock y movimientos

> **Estado:** completada

## Objetivo

Cerrar la trazabilidad del stock: movimientos manuales (entrada/salida/ajuste) con historial filtrable, y el stock de ingredientes pasa a ser **de solo lectura** — solo se mueve por movimientos auditados (los de comandas ya lo hacen desde la guía 11). Reemplaza a `trg_ActualizarStockMovimiento`. Stock es área crítica (guía 00): con tests.

## Semántica y decisiones

| Regla | Viejo (`trg_ActualizarStockMovimiento`) | Nuevo (`StockService`) |
|---|---|---|
| Entrada | suma | Igual |
| Salida | resta (permite negativo) | Igual (realidad de cocina; el estado crítico lo delata) |
| **Ajuste** | **setea** el stock al valor del movimiento | Igual (es inventario: "conté y hay X") |
| Alta de ingrediente con stock inicial | sin auditar | **Mejora**: genera movimiento de entrada "Stock inicial" |
| Edición de stock a mano | permitida (guía 07, temporal) | **Se elimina**: en edición el stock es solo lectura con link a movimientos (promesa de la guía 07) |
| Historial | tabla sin UI propia | Página con filtros por ingrediente, tipo y rango de fechas (últimos 100) |

**Fuera de alcance (documentado):** ABM de clasificaciones y unidades (catálogos sembrados que no cambian, igual que el viejo) y el historial global de modificaciones del dashboard viejo (se consulta por comanda desde la guía 11).

## Pantallas

- **Stock** (sidebar, Admin): ingredientes con stock/mínimo/estado + botón por fila que abre el modal de movimiento (tipo, cantidad, observaciones) + link al historial.
- **Historial de movimientos**: filtros (ingrediente, tipo, desde/hasta) + tabla con fecha, tipo, cantidad, usuario y observaciones.

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/IStockService.cs` / `StockService.cs` | Registrar movimiento transaccional + historial con filtros |
| `Services/IngredienteService.cs` | Alta con movimiento "Stock inicial"; edición ya no toca stock |
| `ViewModels/StockViewModels.cs` | Form de movimiento + item de historial + página |
| `Controllers/StockController.cs` | Solo Admin |
| `Views/Stock/` (Index, Historial) | — |
| `Views/Ingredientes/_Form.cshtml` + Crear/Editar | Stock solo lectura en edición |
| `Views/Shared/_Layout.cshtml` | Ítem Stock habilitado |
| `Recetario-MVC.Tests/StockServiceTests.cs` | Tests del área crítica |
| `Program.cs` | Registro del servicio |

## Tests (`StockServiceTests`)

1. Entrada suma y audita el movimiento con el usuario.
2. Salida resta y permite quedar negativo.
3. Ajuste **setea** el stock al valor indicado (semántica del trigger viejo).
4. Cantidad inválida (≤ 0) o ingrediente inexistente → falla sin tocar nada.
5. El historial filtra por ingrediente y por tipo.
6. El alta de ingrediente con stock inicial genera el movimiento de entrada auditado.

## Verificación

- `dotnet test` en verde (15 previos + los nuevos).
- Por HTTP: entrada de 10 kg de Harina → stock sube y aparece en el historial; ajuste a 5 → stock queda exactamente en 5; el historial filtra por tipo; el form de edición del ingrediente ya no permite tocar el stock.
- Los movimientos de las comandas (guía 11) aparecen en el mismo historial.

## Próximo paso

Fase 5 — [Guía 13: PDFs con QuestPDF](../fase-5-cierre/13-pdfs-questpdf.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar movimientos de stock con historial auditado
- StockService reemplaza a trg_ActualizarStockMovimiento con ajuste por inventario
- Stock de solo lectura en ingredientes y alta con movimiento inicial
- Historial filtrable y tests xUnit
```
