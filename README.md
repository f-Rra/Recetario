# Recetario

Sistema de gestión de recetas, costeo y comandas para cocina, desarrollado para **Grupo Southex** (facility management & food services). Aplicación web ASP.NET Core MVC que permite administrar recetas con su costeo por ingrediente, controlar stock con trazabilidad completa y operar el flujo diario de comandas de cocina.

> Este proyecto nació como TPI académico en **WinForms** (.NET Framework, 3 capas, ADO.NET + stored procedures) y fue **migrado a ASP.NET Core MVC**. El proceso completo está documentado guía por guía en [`Guias/`](Guias/).

## Funcionalidades

**Administración**
- ABM de ingredientes con código autogenerado, estado de stock (normal / bajo / crítico) y baja protegida por referencias.
- ABM de proveedores y **precios por ingrediente con historial** (precio vigente por fecha).
- ABM de recetas: datos, ingredientes con rendimiento (cantidad bruta calculada) y procedimiento paso a paso.
- **Costeo de recetas** escalado por porciones, con desglose por ingrediente persistido e historial reimprimible.
- **Stock y movimientos**: entradas, salidas y ajustes por inventario, con historial filtrable y auditado.
- Gestión de usuarios (roles Admin / Cocina) y responsables de sector.
- Dashboard con métricas y alertas de stock.

**Cocina**
- Panel del día con las comandas y accesos rápidos.
- Registro de comandas que **consume stock** automáticamente, escalado a las porciones.
- Modificaciones de comanda (sustitución / adición / eliminación) con ajuste de stock auditado.
- Consulta de recetas y descarga de la comanda en PDF.

## Stack

| Capa | Tecnología |
|---|---|
| Framework | ASP.NET Core MVC · .NET 9 |
| Datos | Entity Framework Core 9 (code-first, Fluent API) · SQL Server |
| Autenticación | ASP.NET Core Identity (roles Admin / Cocina) |
| UI | Bootstrap 5 · Bootstrap Icons · paleta de marca Grupo Southex |
| PDFs | QuestPDF |
| Tests | xUnit · EF Core InMemory |

La lógica de negocio vive en **servicios** (`Services/`) con interfaces e inyección de dependencias; los controladores son delgados. Los servicios críticos (costeo, comandas, stock) están cubiertos por **22 tests** que verifican paridad con el sistema original.

## Cómo correrlo

Requisitos: .NET 9 SDK y SQL Server (Express o superior).

```bash
cd app/Recetario/Recetario-MVC
dotnet ef database update   # crea la base RecetarioMVC y siembra roles, admin y catálogos
dotnet run
```

La cadena de conexión está en `appsettings.json` (por defecto `.\SQLEXPRESS`). El seeder crea un usuario administrador inicial:

- **Usuario:** `admin@recetario.local`
- **Contraseña:** `Admin123!` _(cambiar en el primer ingreso)_

Los demás usuarios se dan de alta desde el módulo de Usuarios (no hay registro público).

## Estructura del repositorio

```
Recetario/
├─ app/Recetario/
│  ├─ Recetario.sln
│  ├─ Recetario-MVC/          # la aplicación
│  └─ Recetario-MVC.Tests/    # tests de servicios críticos
├─ Guias/                     # bitácora de la migración, una guía por commit
├─ scripts/                   # schema y objetos SQL originales (SPs, triggers, vistas)
└─ README.md
```

`scripts/` conserva la base de datos y la lógica del sistema WinForms original. Se mantiene como documentación: fue la referencia contra la que se verificó cada servicio reescrito en C# (por ejemplo, el costeo replica la selección de precio vigente del `sp_CalcularCostoReceta`, y el consumo de stock replica `trg_ConsumirStockComanda`).

## Sobre la migración

La migración se hizo en 5 fases (esqueleto → CRUDs → núcleo/costeo → cocina → soporte → cierre), documentadas en [`Guias/`](Guias/). Además de trasladar la funcionalidad, se corrigieron limitaciones del diseño original: historial real de precios, escalado de costeo por porciones, bloqueo de costeos incompletos, y trazabilidad completa del stock mediante movimientos auditados.
