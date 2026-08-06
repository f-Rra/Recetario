# Guía 29 — Proveedores: fichas de contacto

> **Estado:** completada

## Objetivo

Tercer commit del rediseño de Admin. El listado pasa a ser una **grilla de
fichas** con los datos para contactar al proveedor, y el alta y la edición
dejan de ser pantallas propias.

De tres pantallas —listado, alta, edición— queda una.

## Decisiones de diseño (cerradas con maquetas)

- **Fichas en grilla de tres**: nombre, contacto, y debajo teléfono, email y
  dirección con **íconos de línea del mismo gris que el texto secundario**, no
  emojis de color.
- **Los datos que faltan se muestran igual**, en gris: "sin email", "sin
  dirección". Un proveedor a medio cargar se ve de una, en vez de esconderse
  detrás de un guion.
- La **cantidad de precios** va como badge: celeste cuando tiene, y en cero
  avisa que ese proveedor todavía no aporta nada al costeo.
- **Alta y edición en modal**, con la misma mecánica que ingredientes: si el
  formulario vuelve con errores, la pantalla se rearma y el modal se reabre.
- Los botones quedan **pegados al pie** de la ficha, así se alinean aunque las
  fichas tengan distinto alto.

## Un solo modal de edición para todas las fichas

Con un modal por proveedor la página crecería con cada uno. Hay uno solo: la
ficha que se toca le pasa sus datos por `data-*` y el JS los carga. Cuando el
modal se reabre **por un error**, no hay ficha que lo dispare, así que el
handler corta si no hay `relatedTarget` y quedan los valores que ya puso el
servidor.

Como en ingredientes, la edición se bindea con `[Bind(Prefix = "Edicion")]`
para no compartir los nombres de campo con el alta.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/ProveedorViewModels.cs` | `ProveedoresPaginaViewModel`, `Direccion` en el listado |
| `Services/ProveedorService.cs` | La dirección viaja con el listado |
| `Controllers/ProveedoresController.cs` | Todo vuelve al Index; se van los GET de `Crear` y `Editar` |
| `Views/Proveedores/Index.cshtml` | Grilla de fichas con sus tres modales |
| `Views/Proveedores/_Form.cshtml` | Sirve al alta y a la edición; suma el id oculto |
| `Views/Proveedores/Crear.cshtml` · `Editar.cshtml` | **Eliminadas** |
| `wwwroot/css/site.css` | Lenguaje `prv-*` |

## Verificación

- Las tres fichas muestran teléfono, email y dirección, y los íconos se ven en
  gris, no en color.
- El alta abre en modal; sin nombre o con un email inválido vuelve con el
  error y el modal abierto.
- Tocar Editar en una ficha carga sus datos en el modal.
- Eliminar un proveedor sin precios funciona; con precios lo rechaza y explica
  por qué.
- Las rutas viejas (`Crear`, `Editar/1`) devuelven 404.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): rediseñar proveedores como fichas de contacto
- Grilla de tres con los datos de contacto y lo que falta en gris
- Alta y edición en modales, sin cambiar de pantalla
```
