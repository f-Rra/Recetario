# Guía 30 — Responsables: una tarjeta por sector

> **Estado:** completada

## Objetivo

Cuarto commit del rediseño de Admin. La tabla de responsables pasa a ser una
**tarjeta por sector**, y el alta y la edición dejan de ser pantallas propias.

El cambio de fondo no es visual: la tabla vieja listaba gente, pero la pregunta
que importa es **si cada sector está cubierto**. Un sector sin responsable hace
que sus comandas se le asignen a otro (guía 11), y eso hoy se descubre cuando
ya pasó.

## Decisiones de diseño (cerradas con maquetas)

- **Una tarjeta por clasificación**, con quienes la cubren. Vienen **todas**,
  también las vacías: son las que van a sorprender.
- Los **sectores sin cubrir** van con borde y título rojos, con el aviso de qué
  va a pasar y un botón **Asignar** que abre el alta con ese sector ya elegido.
- **Resumen arriba**: "8 responsables · 1 sector sin cubrir", en rojo cuando
  hay alguno. Es el dato que antes había que contar a ojo.
- Un bloque aparte para los cargados **sin sector**, que no reciben ninguna
  comanda. La tabla vieja los mostraba con un guion en la columna Sector y
  pasaban desapercibidos — en los datos demo había uno.
- Cada persona muestra teléfono, email y **cuántas comandas lleva**; lo que
  falta va en gris, como en proveedores.
- **Alta y edición en modal**, con la misma mecánica que ingredientes y
  proveedores: si el formulario vuelve con errores, el modal se reabre.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/PersonaViewModels.cs` | `SectorConResponsables`, `ResponsablesPorSector`, `ResponsablesPaginaViewModel` |
| `Services/IPersonaService.cs` / `PersonaService.cs` | `ListarPorSectorAsync` |
| `Controllers/ResponsablesController.cs` | Todo vuelve al Index; se van los GET de `Crear` y `Editar` |
| `Views/Responsables/Index.cshtml` | Grilla de sectores con sus tres modales |
| `Views/Responsables/_Persona.cshtml` | Una persona dentro de su tarjeta |
| `Views/Responsables/_Form.cshtml` | Sirve al alta y a la edición; suma el id oculto |
| `Views/Responsables/Crear.cshtml` · `Editar.cshtml` | **Eliminadas** |
| `wwwroot/css/site.css` | Lenguaje `rsp-*` |
| `Recetario-MVC.Tests/PersonaServiceTests.cs` | El agrupado por sector |

## Verificación

- Los seis sectores aparecen; Decoración, que está vacío, va en rojo.
- "Asignar" abre el alta con ese sector ya seleccionado.
- Al asignarle alguien, el aviso del resumen desaparece; al quitarlo, vuelve.
- Un responsable sin sector aparece en el bloque de abajo.
- Eliminar a alguien sin comandas funciona; con comandas lo rechaza.
- Las rutas viejas (`Crear`, `Editar/1`) devuelven 404.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): mostrar los responsables por sector
- Una tarjeta por clasificación, con los sectores sin cubrir marcados
- Bloque aparte para quienes no tienen sector asignado
- Alta y edición en modales, sin cambiar de pantalla
```
