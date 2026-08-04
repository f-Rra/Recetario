# Guía 21 — Rediseño: detalle de comanda

> **Estado:** pendiente

## Objetivo

Tercera pantalla del rediseño (maquetas de agosto 2026): el detalle con los
ingredientes en una sola columna legible y el resto a la derecha.

## Decisiones de diseño (cerradas con maquetas)

- **Encabezado como tarjeta**: nombre de la receta + línea gris con sector,
  fecha y quién la generó; las **porciones como número grande** a la derecha y
  el botón Descargar PDF. **Sin el responsable en el encabezado** (sigue en el
  PDF).
- **Dos columnas**: a la izquierda la tarjeta INGREDIENTES con la lista en
  **una columna con puntos de guía** (nombre … cantidad), de arriba a abajo; a
  la derecha, apiladas, MODIFICACIONES (etiquetas celestes) y PROCEDIMIENTO
  (pasos numerados con el círculo celeste).
- Títulos de tarjeta en mayúsculas chicas azules espaciadas.

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Comandas/Detalle.cshtml` | Reescritura con el encabezado y las dos columnas |
| `wwwroot/css/site.css` | Estilos del detalle (`dtc-*`) reutilizando la columna con puntos de la guía 19 |

## Verificación

- Los ingredientes se leen en una sola columna con las cantidades alineadas.
- Modificaciones y procedimiento quedan a la derecha; sin responsable arriba.
- El PDF se descarga igual que siempre.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar el detalle de comanda
- Ingredientes en una columna con puntos de guía
- Modificaciones y procedimiento a la derecha
```
