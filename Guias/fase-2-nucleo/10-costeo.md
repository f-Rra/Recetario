# Guía 10 — Costeo

> **Estado:** completada

## Objetivo

Reescribir `sp_CalcularCostoReceta` como servicio C# testeado — la lógica más crítica del sistema (guía 00: es una de las tres áreas con tests obligatorios). Incluye la pantalla de costeo con desglose por ingrediente y el historial persistido.

## Semántica del SP original y qué cambia

| Regla | SP viejo | Servicio nuevo |
|---|---|---|
| Precio vigente | `ROW_NUMber` por ingrediente: `FechaVigencia DESC`, empate por **menor IdProveedor** | Igual por fecha; el empate lo gana **el último precio cargado** (`IdPrecio` más alto) — coherente con el badge "Vigente" de la guía 08. Con el historial real de precios el empate por proveedor ya no tiene sentido |
| Porciones | **No escala cantidades**: costea siempre la receta base y solo divide (`unitario = total / porciones`) → pedir más porciones "abarataba" la porción | **Mejora**: las cantidades se escalan por `porciones / porcionesBase`; el costo unitario queda invariante (como corresponde) y coincide con el SP cuando `porciones = porcionesBase` |
| Ingrediente sin precio | Si **ninguno** tiene precio → error; si **algunos** tienen → los saltea **en silencio** y costea incompleto | **Mejora**: el cálculo se bloquea listando qué ingredientes no tienen precio — nunca se registra un costeo incompleto |
| Persistencia | Solo cabecera (`Costos`) | Cabecera + **desglose** (`CostosDetalle`, guía 03): el histórico sobrevive a los cambios de precios |
| Redondeos | `DECIMAL(12,4)` implícito | `Math.Round(4)` explícito en cantidades escaladas, total y unitario |

## Flujo de la pantalla

Desde Recetas (listado o detalle, solo Admin) → **Costear**: elegir porciones (default: las base) → **Calcular** muestra el desglose (ingrediente, cantidad bruta escalada, precio vigente, subtotal) sin guardar → **Registrar costeo** persiste cabecera + desglose. Debajo, el historial de costeos de la receta.

## Piezas

| Archivo | Contenido |
|---|---|
| `Services/ICosteoService.cs` / `CosteoService.cs` | `CalcularAsync` (sin efectos), `RegistrarAsync` (recalcula y persiste), `HistorialAsync` |
| `ViewModels/CosteoViewModels.cs` | Resultado con desglose, faltantes de precio e historial |
| `Controllers/CostosController.cs` | Solo Admin: `Costear` (GET), `Calcular` (POST sin guardar), `Registrar` (POST) |
| `Views/Costos/Costear.cshtml` | Form + desglose + historial |
| `Views/Recetas/Index.cshtml` / `Detalle.cshtml` | Botón "Costear" (Admin) |
| **`Recetario-MVC.Tests/`** | **Proyecto xUnit nuevo** (EF Core InMemory), agregado a la solución |
| `Program.cs` | Registro del servicio |

## Tests (`CosteoServiceTests`)

1. **Paridad con el caso real cargado**: Ñoquis (0,625 kg bruta de Harina × $1.200,50, 6 porciones base) → total `750,3125`, unitario `125,0521` — verificable a mano y contra el SP.
2. Precio vigente: gana la fecha más reciente; con fechas empatadas gana el último cargado.
3. Escalado: 12 porciones (2× base) → total ×2, **unitario invariante**.
4. Ingrediente sin precio → bloquea e informa cuál.
5. Receta inexistente o porciones ≤ 0 → error.
6. `RegistrarAsync` persiste cabecera y desglose con los valores calculados.

## Verificación

- `dotnet test` → todos los tests en verde.
- En la app: costear Ñoquis con 6 porciones → `$750,31` total; registrar → aparece en el historial; el dashboard actualiza "costo promedio por porción".
- Costear con un ingrediente sin precio (agregando uno nuevo a la receta) → mensaje con el faltante.

## Próximo paso

Fase 3 — [Guía 11: Comandas y panel de cocina](../fase-3-cocina/11-comandas-cocina.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar costeo de recetas con desglose y tests
- CosteoService reemplaza a sp_CalcularCostoReceta con escalado por porciones
- Bloqueo ante ingredientes sin precio y desglose persistido
- Proyecto xUnit con tests del servicio de costeo
```
