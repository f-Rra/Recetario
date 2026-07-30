# Guía 17 — Depósitos de stock

> **Estado:** completada

## Objetivo

Separar el stock en los dos lugares donde realmente se guarda en la cocina:
**bodega** (todo lo seco) y **cámara** (todo lo refrigerado). Al terminar, cada
ingrediente pertenece a un depósito, la pantalla de stock los muestra en
pestañas separadas, y se puede hacer el conteo físico de un depósito por vez.

## Decisiones

- **Un depósito por ingrediente**, no stock repartido entre los dos: lo seco va a
  bodega y lo refrigerado a cámara, así que el lugar es una característica del
  ingrediente. Se resuelve con una columna nueva en `Ingredientes`, sin tablas
  intermedias ni cambios en comandas ni movimientos.
- **Los depósitos son fijos**: enum `Deposito { Bodega, Camara }`, igual que los
  tipos de movimiento. Agregar un tercero pediría una migración.
- La pantalla de stock usa **pestañas** Bodega / Cámara: una tabla a la vez, sin
  scroll, y la búsqueda aplica a las dos.
- Se agrega **inventario por depósito**: una pantalla para contar todo un depósito
  de una vez, cargando las cantidades reales y generando los ajustes juntos. Hoy
  el ajuste es de a un ingrediente por el modal, que para un inventario completo
  es incómodo.

## Cambios en la base

```
Ingredientes
  + Deposito  int  NOT NULL  DEFAULT 1 (Bodega)
```

Migración `AgregarDepositoAIngrediente`. Los ingredientes existentes quedan en
bodega y un script reasigna los refrigerados a cámara: lácteos, carnes, huevos y
verduras frescas. Los secos, aceites, especias, conservas, papas, cebolla y ajo
quedan en bodega.

## Piezas

| Archivo | Contenido |
|---|---|
| `Models/Enums.cs` | Enum `Deposito` |
| `Models/Ingrediente.cs` | Propiedad `Deposito` |
| `Data/Configuraciones/IngredienteConfiguracion.cs` | Requerido, con Bodega por defecto |
| `Migrations/` | `AgregarDepositoAIngrediente` |
| `Helpers/NombreDeposito.cs` | Nombre para mostrar ("Bodega", "Cámara") |
| `Services/IIngredienteService.cs` / `IngredienteService.cs` | El depósito entra en el alta, la edición y el listado |
| `Services/IStockService.cs` / `StockService.cs` | `ObtenerInventarioAsync` y `GuardarInventarioAsync` |
| `ViewModels/IngredienteViewModels.cs` | Depósito en el form y en el listado |
| `ViewModels/StockViewModels.cs` | Inventario por depósito |
| `Controllers/StockController.cs` | `Inventario` (GET y POST), solo Admin |
| `Views/Stock/Index.cshtml` | Pestañas Bodega / Cámara |
| `Views/Stock/Inventario.cshtml` | Conteo de un depósito |
| `Views/Ingredientes/` | Depósito en el formulario y en el listado |
| `scripts/Datos_Demo_MVC.sql` | Asignación de depósitos |
| `Recetario-MVC.Tests/StockServiceTests.cs` | Tests del inventario |

## Reglas del inventario

- Se elige un depósito y se listan sus ingredientes con el stock que dice el
  sistema y un campo para la cantidad contada.
- Las filas que se dejan vacías **no se tocan**: sirve para contar de a partes.
- Cada fila con un valor distinto al del sistema genera un movimiento de **ajuste**
  (la misma semántica que ya existe: el stock queda en lo contado) con la
  observación "Inventario de bodega/cámara".
- Todo se guarda en una transacción y el aviso dice cuántos ingredientes se
  ajustaron.

## Verificación

- La migración aplica y los 27 ingredientes quedan repartidos entre los dos depósitos.
- El stock muestra las dos pestañas con su cantidad de ingredientes; la búsqueda
  filtra dentro de ambas.
- Un ingrediente nuevo pide el depósito; editarlo permite cambiarlo.
- El inventario de bodega con dos cantidades cambiadas y el resto vacío ajusta
  solo esos dos y deja el resto intacto, con sus movimientos en el historial.
- `dotnet test` en verde.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): separar el stock en bodega y cámara
- Cada ingrediente pertenece a un depósito
- Pantalla de stock con pestañas por depósito
- Inventario para contar un depósito completo de una vez
```
