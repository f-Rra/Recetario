# Guía 11 — Comandas y panel de cocina

> **Estado:** completada

## Objetivo

El módulo operativo del rol Cocina: registrar comandas (que consumen stock), modificarlas (sustituir/agregar/quitar ingredientes) y el panel del día. Reemplaza a `sp_RegistrarComanda` + los 3 triggers de stock con lógica de servicio transaccional y testeada (comandas y stock son área crítica según la guía 00).

## Semántica del sistema viejo y qué cambia

| Regla | Viejo (SP + triggers) | Nuevo (ComandaService) |
|---|---|---|
| Responsable | Si no se indica persona, asigna el primer responsable del sector (= clasificación de la receta) | Igual, pero la pantalla muestra el combo con el responsable sugerido preseleccionado |
| Consumo de stock | Trigger genera `MovimientosStock` de salida por ingrediente: `(CantBruta / PorcionesBase) × Porciones` | Misma fórmula (redondeo explícito a 4), en **una transacción** con la comanda |
| Modificaciones | Trigger ajusta `StockActual` directo, **sin registrar movimientos** (sin auditoría) | **Mejora**: cada modificación genera sus movimientos (entrada por devolución, salida por consumo) con observación que referencia la comanda |
| Tipos de modificación | Tabla `TiposModificacion` | Enum (guía 03): **Sustitución** (original + reemplazo), **Adición** (solo reemplazo), **Eliminación** (solo original) — validado por tipo |
| Stock negativo | Permitido | Permitido (realidad de cocina); el dashboard lo muestra como crítico |
| Comandas | Sin edición ni borrado | Igual: la comanda es un registro operativo inmutable; lo único que se le agrega son modificaciones |
| Ingredientes escalados | `vw_Comanda` + `sp_AjustarReceta` | El detalle de la comanda muestra los ingredientes escalados a sus porciones |

## Requisito descubierto: Responsables

`Comandas.IdPersona` es obligatorio y las personas (responsables de sector) no tienen ABM en la web. Esta guía incluye un **mini-ABM de Responsables** (nombre, apellido, contacto, sector = clasificación), solo Admin, en la sección Sistema de la sidebar. Sin responsables cargados la pantalla de comandas lo avisa con un link.

## Pantallas

- **Comandas** (sidebar, ambos roles): listado por fecha (default hoy) + botón Registrar.
- **Registrar comanda**: receta activa, porciones, responsable (sugerido por sector). Al confirmar: comanda + consumo de stock.
- **Detalle de comanda**: datos, ingredientes escalados a las porciones, modificaciones registradas + form para agregar una (con validación por tipo).
- **Panel de cocina** (Home del rol Cocina, reemplaza a la bienvenida): métricas del día (comandas, porciones) + comandas de hoy + acceso rápido a registrar y a recetas.

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/IComandaService.cs` / `ComandaService.cs` | Registrar (transaccional), listar por fecha, detalle con escalado, agregar modificación con stock auditado, panel del día |
| `Services/IPersonaService.cs` / `PersonaService.cs` | Mini-ABM de responsables |
| `ViewModels/ComandaViewModels.cs` / `PersonaViewModels.cs` | — |
| `Controllers/ComandasController.cs` | Cocina y Admin |
| `Controllers/ResponsablesController.cs` | Solo Admin |
| `Views/Comandas/` (Index, Registrar, Detalle) y `Views/Responsables/` | — |
| `Views/Home/Cocina.cshtml` | Panel del día |
| `Views/Shared/_Layout.cshtml` | Comandas habilitado (ambos); Responsables en Sistema (Admin) |
| `Recetario-MVC.Tests/ComandaServiceTests.cs` | Tests del área crítica |
| `Program.cs` | Registro de servicios |

## Tests (`ComandaServiceTests`)

1. Registrar comanda descuenta stock escalado: receta 6 porciones con 0,625 kg brutos, comanda de 12 → salida de 1,25 kg y stock actualizado.
2. Registrar genera los `MovimientosStock` de salida con la observación de la comanda.
3. Sin responsable del sector → falla con mensaje (regla del SP viejo).
4. Sustitución: devuelve stock del original, consume del reemplazo, y **ambos movimientos quedan auditados**.
5. Adición y Eliminación validan qué ingrediente requiere cada tipo.
6. Receta inactiva o porciones inválidas → falla.

## Verificación

- `dotnet test` en verde (8 de costeo + los nuevos).
- Flujo completo por HTTP: crear responsable → registrar comanda de Ñoquis ×12 → stock de Harina baja 1,25 → movimiento "Consumo comanda #1" → modificación de eliminación devuelve stock → panel de cocina muestra la comanda del día.
- El listado de comandas filtra por fecha; el detalle muestra ingredientes escalados.

## Próximo paso

Fase 4 — [Guía 12: Stock y movimientos](../fase-4-soporte/12-stock-y-movimientos.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar comandas con consumo de stock y panel de cocina
- ComandaService transaccional reemplaza a sp_RegistrarComanda y triggers
- Modificaciones por tipo con movimientos de stock auditados
- Panel del día para Cocina, ABM de responsables y tests xUnit
```
