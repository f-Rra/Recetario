# Guía 13 — PDFs con QuestPDF

> **Estado:** completada

## Objetivo

Reemplazar al `GeneradorPDF` de iTextSharp (WinForms) con QuestPDF: los dos documentos del sistema viejo, generados en el servidor y descargados desde el navegador, con la identidad visual Southex.

| Documento | Viejo | Nuevo |
|---|---|---|
| **Comanda de cocina** | Secciones con receta, sector, responsable, ingredientes, modificaciones y procedimiento | Igual, desde el detalle de la comanda, con los ingredientes **escalados a las porciones** y el procedimiento de la receta |
| **Informe de costeo** | Desglose + totales del cálculo en memoria | Desde el **historial persistido** (`CostosDetalle`, guía 03): cualquier costeo registrado se reimprime fiel aunque los precios hayan cambiado — acá rinde la mejora del desglose persistido |

## Decisiones

- **QuestPDF** con licencia Community (`QuestPDF.Settings.License = LicenseType.Community`, válida para organizaciones con ingresos < USD 1M).
- Los documentos usan la paleta Southex (encabezados azul `#1078B0`, acentos celestes) y tipografía del sistema.
- Endpoints que devuelven `FileResult` (`application/pdf`) con nombre descriptivo (`comanda-3.pdf`, `costeo-REC001-2026-07-14.pdf`); el navegador decide mostrar o descargar.
- El detalle de comanda ahora incluye el **procedimiento** de la receta (lo necesita el PDF y le sirve a la pantalla).

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/Pdf/ComandaPdf.cs` | Documento QuestPDF de la comanda |
| `Services/Pdf/CosteoPdf.cs` | Documento QuestPDF del costeo registrado |
| `Services/ICosteoService.cs` / `CosteoService.cs` | Nuevo `ObtenerRegistradoAsync(idCosto)` que lee cabecera + desglose persistido |
| `Services/ComandaService.cs` | El detalle incluye pasos de la receta |
| `ViewModels/` | `CosteoRegistradoViewModel`; `Pasos` en el detalle de comanda |
| `Controllers/ComandasController.cs` | `Pdf(id)` |
| `Controllers/CostosController.cs` | `Pdf(idCosto)` |
| `Views/Comandas/Detalle.cshtml` | Botón "Descargar PDF" + sección procedimiento |
| `Views/Costos/Costear.cshtml` | Botón PDF por fila del historial |
| `Program.cs` | Licencia QuestPDF |

## Verificación

- `dotnet test` sigue en verde (los PDFs no tocan lógica testeada).
- `GET /Comandas/Pdf/1` → 200, `application/pdf`, contenido empieza con `%PDF` y pesa > 10 KB.
- `GET /Costos/Pdf/1` → ídem, con los valores del costeo registrado ($750,31).
- Abrir ambos en el navegador y revisar visualmente (esto queda para el checkpoint del usuario).
- Rol Cocina puede descargar el PDF de comanda; el de costeo es solo Admin.

## Próximo paso

[Guía 14 — Retiro del WinForms](14-retiro-del-winforms.md), la última.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar PDFs de comanda y costeo con QuestPDF
- Comanda de cocina con ingredientes escalados y procedimiento
- Informe de costeo desde el desglose persistido
- Reemplaza al GeneradorPDF de iTextSharp
```
