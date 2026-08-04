# Guía 20 — Rediseño: historial de comandas

> **Estado:** pendiente

## Objetivo

Segunda pantalla del rediseño (maquetas de agosto 2026): el historial como
**tabla en azul**, sin cambios de funcionalidad.

## Decisiones de diseño (cerradas con maquetas)

- **Resumen arriba**: chip celeste con la cantidad de comandas del filtro,
  junto a los filtros de fecha y receta y el botón **Filtrar azul sólido**.
- **Tabla con encabezado azul pleno** (letras blancas espaciadas) y
  **separadores de día** como filas celestes ("LUNES 3 DE AGOSTO").
  Desaparece la columna Fecha: el día vive en el separador.
- **Recetas en azul**: el nombre es el enlace al detalle — **la fila entera es
  clickeable** y se elimina el botón Ver. Queda un solo botón por fila: PDF.
- **Modificaciones centradas**: la descripción como etiqueta celeste
  (una sola: su texto; varias: "N modificaciones") y el guion de "sin
  modificaciones" centrado en la columna.
- Columnas: Receta · Sector · Porciones · Responsable · Modificaciones · PDF.
- Se quita la columna Usuario de la tabla (queda en el detalle).

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Comandas/Index.cshtml` | Reescritura con la tabla azul y días agrupados |
| `ViewModels/ComandaViewModels.cs` | Descripción de la primera modificación en el ítem de lista (si hace falta) |
| `Services/ComandaService.cs` | El listado trae el texto de la modificación única |
| `wwwroot/css/site.css` | Estilos de la tabla azul (`hst-*`) |

## Verificación

- Las comandas se agrupan por día con el separador celeste; el orden es el de
  siempre (más recientes arriba).
- Clic en la fila abre el detalle; el botón PDF descarga la reimpresión.
- Los filtros de fecha y receta siguen funcionando; el chip del resumen cuenta
  lo filtrado.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar el historial de comandas como tabla azul
- Días agrupados con separadores y resumen del filtro
- La fila abre el detalle; queda solo el botón de PDF
```
