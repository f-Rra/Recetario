# Guía 09 — Recetas

> **Estado:** completada

## Objetivo

El módulo central del dominio: ABM de recetas con sus tres partes — datos, ingredientes con cantidades/rendimiento, y procedimiento paso a paso. Al terminar queda todo listo para el costeo (guía 10), que solo necesita leer `IngredientesReceta` + precios vigentes.

## Qué se mantiene y qué mejora respecto del WinForms (`ucRecetas`)

| Aspecto | WinForms | Web |
|---|---|---|
| Código `REC###` | Autogenerado | Igual (mismo mecanismo que ING de la guía 07) |
| Armado | Todo en un panel: grilla + combos | **Dos pasos**: alta rápida de datos → página de edición con secciones de ingredientes y procedimiento |
| Ingredientes | Agregar/quitar con cant. neta y rendimiento; `CantBruta` calculada | Igual; la cantidad bruta se calcula en el servidor (`CantNeta / (Rendimiento/100)`) y se muestra en la tabla. Ingrediente duplicado se rechaza |
| Pasos | Agregar/quitar | Igual + **renumeración automática** al quitar (el schema nuevo tiene índice único por receta+paso) |
| Baja | Directa | **Protegida**: con comandas o costos históricos no se elimina; para sacarla de circulación está **Activo/Inactivo** (toggle) |
| Consulta | — | Página **Detalle de solo lectura** (datos + ingredientes + procedimiento), visible también para el rol Cocina — la va a usar el panel de cocina (guía 11) |
| Imagen | Campo `Imagen` en schema | **Se omite por ahora** (queda el campo en el modelo); subir archivos se evalúa como mejora al final |
| Filtro | Por clasificación | Búsqueda por código/nombre + filtro por clasificación combinables |

## Permisos

- `Index` y `Detalle`: cualquier usuario logueado (Cocina consulta recetas).
- `Crear`, `Editar` (y las acciones de ingredientes/pasos), `Eliminar`: solo Admin.
- El ítem **Recetas** de la sidebar se habilita para ambos roles; los botones de acción se ocultan según rol.

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/IRecetaService.cs` / `RecetaService.cs` | Listar (búsqueda + clasificación), detalle, form, generar código, crear, editar datos, eliminar protegida, agregar/quitar ingrediente, agregar/quitar paso con renumeración |
| `ViewModels/RecetaViewModels.cs` | Lista, form de datos, detalle completo, forms inline de ingrediente y paso |
| `Controllers/RecetasController.cs` | Permisos por acción como arriba |
| `Views/Recetas/Index.cshtml` | Búsqueda + filtro clasificación + badges Activa/Inactiva |
| `Views/Recetas/Crear.cshtml` | Alta de datos; redirige a Editar para cargar el contenido |
| `Views/Recetas/Editar.cshtml` | Tres secciones: datos / ingredientes / procedimiento |
| `Views/Recetas/Detalle.cshtml` | Solo lectura (Cocina) |
| `Views/Shared/_Layout.cshtml` | Ítem Recetas habilitado para ambos roles |
| `Program.cs` | Registro del servicio |

## Verificación

- Crear receta → código `REC001`, redirige a edición.
- Agregar ingredientes: neta `0,5` kg con rendimiento `80` → bruta `0,625`; duplicado rechazado con mensaje.
- Agregar 3 pasos, quitar el 2° → renumera (1, 2).
- Editar datos (nombre, porciones, activo) y verificar en el listado.
- Detalle como Cocina: se ve todo, sin botones de edición; `/Recetas/Crear` denegado.
- Eliminar receta sin historial → OK (con comandas/costos se prueba en fases 3–4).
- Filtro por clasificación + búsqueda combinados.

## Próximo paso

[Guía 10 — Costeo](10-costeo.md): reescritura de `sp_CalcularCostoReceta` con tests xUnit.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar ABM de recetas con ingredientes y procedimiento
- Alta con código REC y edición en secciones (datos, ingredientes, pasos)
- Cantidad bruta calculada por rendimiento y pasos renumerados
- Detalle de solo lectura visible para Cocina y baja protegida
```
