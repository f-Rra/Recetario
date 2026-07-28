# Guía 15 — Comandera de cocina

> **Estado:** completada

## Objetivo

Llevar a la web la pantalla de cocina tal como funciona en el WinForms (`ucDashboardCocina`): **una sola pantalla** donde el cocinero arma un pedido con varias recetas, les carga modificaciones, indica los comensales y genera la comanda — que registra, descuenta stock y descarga el PDF con las recetas escaladas.

La guía 11 había resuelto este módulo con un flujo distinto (panel de métricas + alta de una receta por vez). No representaba el trabajo real de cocina, así que el módulo se rediseña siguiendo el original.

## El flujo del WinForms que se replica

1. Filtrar el catálogo por clasificación y elegir varias recetas (casillas).
2. Al pararse en una receta, ver sus ingredientes.
3. Agregar las tildadas al carrito.
4. Modificar o quitar ítems del carrito.
5. Cargar los comensales (uno solo para todo el pedido).
6. Generar: registra, descuenta stock y produce el PDF con una sección por receta.

## Diseño elegido

Sobre el mockup revisado con el usuario: **catálogo en tabla con casillas**, **carrito en panel lateral fijo** y **modificación en ventana emergente** (equivalente al `frmModificacion`).

```
┌──────────────────────────────────────┬─────────────────────┐
│ Buscar receta…      [Clasificación ▾]│ COMANDA             │
│ ┌────────────────┬─────────────────┐ │ Comensales: [ 50 ]  │
│ │ ☑ Catálogo     │ Ingredientes de │ │ ┌─────────────────┐ │
│ │   (solo activas)│ la seleccionada│ │ │ Ñoquis     ✎ ✕ │ │
│ │ ☑ Ñoquis    6  │ escalados a     │ │ │ 1 modificación  │ │
│ │ ☐ Puré      8  │ comensales +    │ │ ├─────────────────┤ │
│ │ ☐ Flan     10  │ semáforo stock  │ │ │ Milanesa   ✎ ✕ │ │
│ └────────────────┴─────────────────┘ │ └─────────────────┘ │
│       [+ Agregar tildadas]           │ Responsable: M.López│
│                                      │ [⭳ Generar comanda] │
└──────────────────────────────────────┴─────────────────────┘
```

## Reglas de negocio

| Tema | Regla |
|---|---|
| Comensales | Uno solo para todo el pedido, en el encabezado del carrito (visible desde el inicio) |
| Catálogo | Solo recetas **activas**; filtro por clasificación + búsqueda por nombre |
| Ingredientes | De la receta seleccionada, **escalados a los comensales** cargados, con **semáforo**: rojo si el ingrediente está en stock crítico |
| Responsable | **Automático** por sector; una receta sin responsable propio (ej. decoración) hereda el de la receta principal. Se muestra en el carrito |
| Stock insuficiente | **Bloquea** la generación e informa qué ingredientes faltan y cuánto |
| Sustituir | No pide cantidad: el reemplazo hereda la del ingrediente original. Solo pide cantidad si el reemplazo tiene otra unidad |
| Quitar | No pide cantidad |
| Agregar | Única que pide cantidad, expresada como **total para la comanda** |
| Stock y modificaciones | El descuento **refleja las modificaciones**: sustituir devuelve el original y descuenta el reemplazo, quitar no descuenta, agregar descuenta lo indicado |
| PDF | Ingredientes **ya modificados** + bloque "Modificaciones" que aclara qué cambió, antes del procedimiento |
| Al generar | Registra una comanda por receta (como el sistema original), descuenta stock, descarga el PDF y **vacía el carrito** |

## Decisiones técnicas

- **El carrito vive en la sesión del servidor** (`AddSession` en `Program.cs`): las acciones son POST + redirect, sin framework de front. Se pierde al cerrar sesión, igual que el carrito en memoria del WinForms.
- **La previsualización de ingredientes se pide por `fetch`** a un endpoint JSON, para no recargar toda la página al cambiar de receta (equivale al `SelectionChanged` de la grilla). JavaScript propio, sin librerías nuevas.
- **El modelo de datos no cambia**: se sigue registrando una fila en `Comandas` por receta con los mismos comensales, como hacía `sp_RegistrarComanda`. El "pedido con varias recetas" es un concepto de la pantalla.

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/ComanderaViewModels.cs` | Estado del carrito en sesión, ítem del catálogo, ingrediente escalado con estado, form de modificación |
| `Services/IComandaService.cs` / `ComandaService.cs` | Catálogo filtrado, ingredientes escalados, resolución de responsables, cálculo de ingredientes efectivos (base + modificaciones), validación de stock y generación transaccional del pedido completo |
| `Helpers/CarritoSesion.cs` | Leer/guardar el carrito en `HttpContext.Session` |
| `Controllers/ComandasController.cs` | `Comandera` (GET), `IngredientesReceta` (JSON), `AgregarAlCarrito`, `QuitarDelCarrito`, `GuardarModificacion`, `QuitarModificacion`, `Generar` |
| `Views/Comandas/Comandera.cshtml` | La pantalla completa + modal de modificación |
| `Views/Home/Cocina.cshtml` | Se elimina: el home de Cocina pasa a ser la comandera |
| `Views/Comandas/Registrar.cshtml` | Se elimina: lo reemplaza la comandera |
| `Services/Pdf/ComandaPdf.cs` | Pasa a recibir varias secciones, con ingredientes modificados y bloque de modificaciones |
| `Recetario-MVC.Tests/ComandaServiceTests.cs` | Se reescribe para el nuevo flujo |
| `Program.cs` | `AddSession` + `UseSession` |
| `scripts/Datos_Demo_MVC.sql` | Datos de demostración portados de `Datos_Prueba.sql` al schema nuevo |

## Datos de demostración

La pantalla no se puede evaluar con la base vacía, así que se portó el juego de
datos del sistema original (`scripts/Datos_Prueba.sql`) al schema nuevo en
`scripts/Datos_Demo_MVC.sql`: 6 recetas con sus 44 ingredientes y 31 pasos,
27 ingredientes con stock, 3 proveedores con precios y 7 responsables de sector.
El stock inicial queda auditado como movimiento de entrada.

```bash
sqlcmd -S ".\SQLEXPRESS" -d RecetarioMVC -i scripts/Datos_Demo_MVC.sql -f 65001
```

## Diseño de la pantalla

Tablero en dos franjas, elegido sobre cinco alternativas maquetadas:

- **Arriba se elige**: catálogo de recetas (con búsqueda, filtro por clasificación
  y casillas) y, al lado, los ingredientes de la receta marcada, escalados a los
  comensales y con semáforo de stock.
- **Abajo la comanda**, a todo el ancho y como planilla: cada receta con su sector,
  su responsable y sus modificaciones visibles sin desplegar nada.

El encabezado de la comanda va en **azul pleno** (`--sx-azul-oscuro`) en lugar del
celeste del resto: es la parte principal de la pantalla y ahí viven los comensales
y el botón de generar. Cada panel tiene scroll propio, así el catálogo puede crecer
sin descolocar el resto.

## Tests

1. Generar un pedido de dos recetas descuenta el stock de ambas, escalado a los comensales.
2. Sustitución: devuelve el stock del original y descuenta el reemplazo por la misma cantidad.
3. Adición y eliminación ajustan el stock según corresponde.
4. Stock insuficiente bloquea: no se registra ninguna comanda ni movimiento.
5. Una receta sin responsable propio hereda el de la receta principal.
6. Carrito vacío o comensales inválidos → falla con mensaje.

## Verificación

- `dotnet test` en verde.
- Flujo completo en el navegador: filtrar, tildar dos recetas, ver ingredientes escalados, agregar, sustituir un ingrediente, generar → PDF correcto, stock descontado según las modificaciones, carrito vacío.
- Con stock insuficiente, la generación se bloquea con el detalle del faltante.
- Se corrigen los datos de prueba con nombres mal codificados (`%D1oquis`), cargados por error durante la verificación de guías anteriores.

## Pendiente para la próxima guía

Qué hacer con el listado de comandas por día y el detalle (hoy siguen como estaban): si se conservan como historial con reimpresión del PDF, o se reemplazan.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar comandera de cocina con carrito y modificaciones
- Catálogo, ingredientes escalados y carrito en una sola pantalla
- Modificaciones que ajustan el stock y se detallan en el PDF
- Validación de stock y generación transaccional del pedido
```
