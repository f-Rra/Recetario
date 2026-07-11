# Guía 03 — Modelo de datos

> **Estado:** completada

## Objetivo

Traducir el schema de `RecetarioDB` (ver [scripts/RecetarioDB.sql](../../scripts/RecetarioDB.sql)) a entidades EF Core con Fluent API, generar la migración inicial y un seeder. Al terminar, `dotnet ef database update` crea la base `RecetarioMVC` completa y la app levanta con los catálogos y el usuario admin sembrados.

## Mejoras sobre el schema viejo

Aprovechando el code-first se corrigen cuatro problemas del diseño original:

1. **Historial de precios.** `PrecioxIngrediente` tenía PK compuesta `(IdIngrediente, IdProveedor)`: cada cambio de precio pisaba el anterior, y sin embargo `sp_CalcularCostoReceta` usaba `ROW_NUMBER` sobre `FechaVigencia` como si hubiera historial. La nueva `PreciosIngrediente` tiene PK propia (`IdPrecio`): cada cambio es una fila nueva y el historial es real.
2. **Tipos como enums.** `TiposMovimiento` (entrada/salida/ajuste) y `TiposModificacion` (sustitución/adición/eliminación) eran tablas catálogo, pero la lógica de negocio depende de esos valores fijos (entrada suma stock, salida resta). Pasan a enums C# (`TipoMovimiento`, `TipoModificacion`) guardados como `int`.
3. **Detalle de costo persistido.** El desglose por ingrediente de un costeo se calculaba al vuelo y se perdía al cambiar los precios. Nueva tabla `CostosDetalle` (hija de `Costos`) para que el histórico sea fiel.
4. **Usuarios separados de Personas.** `Usuarios`/`Personas` cumplían doble rol: usuarios del sistema y responsables de sector. Los usuarios pasan a Identity (`ApplicationUser`, guía 04, con nombre y apellido propios); `Personas` queda solo como responsable de sector para comandas.

Nota semántica descubierta en los datos: `Clasificaciones` funciona a la vez como clasificación de recetas **y** como sector de cocina (las personas referencian su sector vía `IdClasificacion`). Se mantiene así, documentado en la entidad.

## Entidades (`Models/`)

| Entidad | Tabla | Notas |
|---|---|---|
| `Clasificacion` | Clasificaciones | Nombre único |
| `Unidad` | Unidades | Nombre y abreviatura únicos |
| `Persona` | Personas | Responsable de sector; `IdClasificacion` nullable |
| `Proveedor` | Proveedores | — |
| `Ingrediente` | Ingredientes | Código único; stock `decimal(10,4)`; check `StockMinimo >= 0` |
| `PrecioIngrediente` | PreciosIngrediente | **PK propia** `IdPrecio`; índice `(IdIngrediente, FechaVigencia)`; check `Precio > 0` |
| `Receta` | Recetas | Código único; check `PorcionesBase > 0` |
| `IngredienteReceta` | IngredientesReceta | PK compuesta `(IdReceta, IdIngrediente)`; `CantBruta = CantNeta / (Rendimiento/100)`; checks de rendimiento (0–100] y cantidad |
| `Procedimiento` | Procedimientos | Índice único `(IdReceta, NroPaso)` (el schema viejo permitía pasos duplicados) |
| `Comanda` | Comandas | FK a Receta, Persona y `UsuarioId` (Identity, string) |
| `Modificacion` | Modificaciones | `TipoModificacion` enum; ingredientes original/reemplazo nullables |
| `Costo` | Costos | Cabecera del costeo; FK a `UsuarioId` |
| `CostoDetalle` | CostosDetalle | **Nueva**: desglose por ingrediente |
| `MovimientoStock` | MovimientosStock | `TipoMovimiento` enum; `Fecha` default `GETDATE()` |
| `ApplicationUser` | AspNetUsers | `IdentityUser` + Nombre, Apellido, Activo |

Convenciones: fechas `DATE` → `DateOnly`, `DATETIME` → `DateTime`; precisión decimal idéntica al schema viejo (`10,4` cantidades, `12,4` montos, `5,2` rendimiento); navegaciones tipadas con colecciones inicializadas.

## Configuraciones (`Data/Configuraciones/`)

Un `IEntityTypeConfiguration<T>` por entidad (patrón de Control-Almuerzos): `ToTable` + checks, claves, longitudes máximas (idénticas al schema viejo), índices únicos y relaciones con `DeleteBehavior` explícito:

- `Cascade` solo en composiciones reales: receta→ingredientes/procedimientos, comanda→modificaciones, costo→detalle, ingrediente→precios.
- `Restrict` en todo lo demás para no borrar historial por accidente (igual espíritu que el schema viejo, que no tenía cascadas).

## DbContext y seeder (`Data/`)

- `ApplicationDbContext : IdentityDbContext<ApplicationUser>` con `ApplyConfigurationsFromAssembly`.
- `DbSeeder.SeedAsync()` ejecutado al arrancar la app: aplica migraciones pendientes (`MigrateAsync`), crea roles `Admin`/`Cocina`, usuario `admin@recetario.local` (contraseña temporal de desarrollo, se cambia en el primer login) y catálogos (las 6 clasificaciones y 5 unidades del sistema viejo).
- Los triggers del schema viejo (`trg_ActualizarStock*`, `trg_ConsumirStockComanda`) **no se recrean**: esa lógica pasa a los servicios (guías 11 y 12) dentro de transacciones EF.

## Registro en `Program.cs`

`AddDbContext` (connection string `RecetarioMVC`) + `AddIdentity` con políticas de contraseña y cookie apuntando a `/Acceso/Login` (la UI de login llega en la guía 04; acá solo queda configurado).

## Migración

```bash
dotnet tool install --global dotnet-ef        # si no está
dotnet ef migrations add Inicial
dotnet ef database update
```

## Verificación

- `dotnet build` → 0 errores.
- `dotnet ef database update` crea `RecetarioMVC` en `.\SQLEXPRESS`.
- `dotnet run` → la app levanta y el seeder deja: 2 roles, 1 admin, 6 clasificaciones, 5 unidades (verificable por SQL).

## Próximo paso

[Guía 04 — Identity y roles](04-identity-y-roles.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar modelo de datos EF Core
- Entidades y configuraciones Fluent API del schema completo
- Mejoras: historial de precios, enums de tipos, detalle de costo persistido
- Migración inicial y seeder (roles, admin, catálogos)
```
