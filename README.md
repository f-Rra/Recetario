# Recetario

[![CI](https://github.com/f-Rra/Recetario/actions/workflows/ci.yml/badge.svg)](https://github.com/f-Rra/Recetario/actions/workflows/ci.yml)

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

# La clave del administrador inicial no está en el código: se carga con user-secrets,
# que la guarda en tu usuario de Windows, fuera del repositorio
dotnet user-secrets set "DefaultAdmin:Password" "una-clave-de-8-o-mas-caracteres"

dotnet run   # al arrancar aplica las migraciones y siembra roles, catálogos y el administrador
```

La cadena de conexión está en `appsettings.json` (por defecto `.\SQLEXPRESS`); si tu instancia de SQL Server se llama distinto, sobrescribila con `dotnet user-secrets set "ConnectionStrings:RecetarioMVC" "..."`. El seeder crea un administrador inicial:

- **Usuario:** `admin@recetario.local`
- **Contraseña:** la que cargaste en `DefaultAdmin:Password`

Si la base todavía no tiene administrador y falta la clave, la aplicación **no arranca** y muestra el comando que hay que correr. Una vez creado el administrador, la clave ya no se vuelve a pedir.

Los demás usuarios se dan de alta desde el módulo de Usuarios (no hay registro público).

## Publicar en Azure

Se publica igual que [SCA-MVC](https://github.com/f-Rra/Control-Almuerzos-MVC): un App Service gratuito y una base Azure SQL gratuita. No hace falta cambiar código, porque las migraciones se aplican solas al arrancar. Los nombres exactos de las pantallas del portal pueden variar.

1. **Base de datos.** En el portal de Azure, crear una *Azure SQL Database* con la oferta gratuita, llamada `RecetarioMVC`, con autenticación SQL. En el firewall del servidor, permitir el acceso de los servicios de Azure.
2. **Aplicación.** Crear una *Web App* con runtime **.NET 9** y plan **Free (F1)**.
3. **Configuración de la aplicación** (variables de entorno de la Web App):
   - `ConnectionStrings__RecetarioMVC`: la cadena de conexión de la base de Azure SQL.
   - `DefaultAdmin__Password`: la clave del administrador, de 8 caracteres o más. **Sin esta variable la aplicación no arranca** la primera vez.
4. **Publicar.** Desde Visual Studio (clic derecho en el proyecto → *Publicar* → *Azure*) o con `dotnet publish -c Release` y subiendo el resultado.
5. **Primer arranque.** Crea las tablas, los roles, los catálogos y el administrador `admin@recetario.local`. Si algo falla, el *Log stream* de la Web App muestra el motivo.
6. **Datos de demostración** (opcional). Ejecutar `scripts/Datos_Demo_MVC.sql` contra la base de Azure con SSMS o Azure Data Studio, porque el editor de consultas del portal no entiende `GO`. El script borra y recarga los datos operativos: no usarlo en una base con datos reales.

Al ser un plan gratuito, la primera visita después de un rato sin uso puede tardar unos segundos en responder.

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
