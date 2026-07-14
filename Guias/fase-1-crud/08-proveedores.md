# Guía 08 — Proveedores y precios

> **Estado:** completada

## Objetivo

Segundo módulo de la fase 1, repitiendo el molde de la guía 07: ABM de proveedores y, con eso disponible, la carga de **precios por ingrediente con historial** — la mejora estrella del modelo nuevo (guía 03), que el costeo (guía 10) va a consumir.

## Decisiones

- **Proveedores**: nombre obligatorio; contacto, teléfono, email y dirección opcionales (como el schema). Sin código autogenerado (el viejo tampoco tenía). Búsqueda por nombre/contacto.
- **Baja protegida**: un proveedor con precios registrados no se puede eliminar (el historial de precios es la base del costeo). Mensaje claro con la cantidad.
- **Los precios viven en Ingredientes**: cada fila del listado de ingredientes gana un botón `$` que abre "Precios de {ingrediente}": historial (proveedor, precio, fecha de vigencia) + form de alta. El precio **vigente** (fecha más reciente, misma semántica que el `ROW_NUMBER` de `sp_CalcularCostoReceta`) se marca con badge.
- Se permite **eliminar un precio puntual** (corrección de errores de carga); la fila vigente pasa a ser la siguiente más reciente.
- Fecha de vigencia por defecto: hoy. Se permiten fechas pasadas (carga retroactiva).

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/IProveedorService.cs` / `ProveedorService.cs` | Listar con búsqueda, obtener, crear, editar, eliminar protegida |
| `Services/IPrecioIngredienteService.cs` / `PrecioIngredienteService.cs` | Historial por ingrediente, agregar, eliminar, listar proveedores para el select |
| `ViewModels/ProveedorViewModels.cs` | Form + item de lista |
| `ViewModels/PrecioIngredienteViewModels.cs` | Página de precios (ingrediente + historial + form de alta) |
| `Controllers/ProveedoresController.cs` | Solo Admin: Index, Crear, Editar, Eliminar |
| `Controllers/IngredientesController.cs` | Se agregan `Precios` (GET), `AgregarPrecio` y `EliminarPrecio` (POST) |
| `Views/Proveedores/` | Index + Crear/Editar con `_Form` compartida |
| `Views/Ingredientes/Precios.cshtml` | Historial + alta de precio |
| `Views/Ingredientes/Index.cshtml` | Botón `$` por fila |
| `Views/Shared/_Layout.cshtml` | Se habilita el ítem Proveedores |
| `Program.cs` | Registro de los dos servicios |

## Verificación

- ABM proveedores completo (crear, buscar, editar) con validaciones en español.
- Agregar dos precios al mismo ingrediente con fechas distintas → el más reciente lleva el badge **Vigente**; los importes se cargan con coma.
- Intentar eliminar un proveedor con precios → bloqueado con mensaje; sin precios → se elimina.
- Eliminar un precio → el vigente pasa al anterior.
- Cocina: sin ítems en sidebar, `/Proveedores` denegado.

## Próximo paso

Fase 2 — [Guía 09: Recetas](../fase-2-nucleo/09-recetas.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar ABM de proveedores y precios por ingrediente
- Proveedores con búsqueda y baja protegida por precios
- Historial de precios por ingrediente con precio vigente
- Alta de precios desde el listado de ingredientes
```
