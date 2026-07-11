# Guía 02 — Setup del proyecto

> **Estado:** completada

## Objetivo

Crear el proyecto web `Recetario-MVC` dentro de la solución existente, conviviendo con los tres proyectos WinForms hasta el final de la migración (regla de la [guía 00](00-vision-y-decisiones.md)). Al terminar, la solución compila y la app web levanta con el template default.

```
app/Recetario/
  Recetario.sln          ← pasa a tener 4 proyectos
  Dominio/               ← WinForms (se retira en fase 5)
  Negocio/               ← WinForms (se retira en fase 5)
  Presentacion/          ← WinForms (se retira en fase 5)
  Recetario-MVC/         ← nuevo
```

## Pasos

### 1. Crear el proyecto y sumarlo a la solución

```bash
cd app/Recetario
dotnet new mvc -n RecetarioMVC -o Recetario-MVC -f net9.0
dotnet sln Recetario.sln add Recetario-MVC/RecetarioMVC.csproj
```

**Decisión de nombres:** la carpeta es `Recetario-MVC` (guía 00) pero el proyecto/namespace es `RecetarioMVC`, porque C# no admite guiones en namespaces. Mismo criterio que `SCA-MVC` en Control-Almuerzos.

### 2. Paquetes

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version "9.0.*"
dotnet add package Microsoft.EntityFrameworkCore.Tools --version "9.0.*"
dotnet add package Microsoft.EntityFrameworkCore.Design --version "9.0.*"
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version "9.0.*"
```

> ⚠️ **Gotcha ya verificado:** sin `--version`, NuGet instala EF Core 10.x, que requiere .NET 10 y falla con `NU1202`. Se fija la línea `9.0.*` (flotante dentro de 9.x) para acompañar los patches de .NET 9.

QuestPDF y xUnit se agregan recién en sus guías (13 y 10): no cargar dependencias antes de usarlas.

### 3. Estructura de carpetas

Espeja la de Control-Almuerzos-MVC:

| Carpeta | Rol | Reemplaza a (WinForms) |
|---|---|---|
| `Models/` | Entidades del dominio | `Dominio/` |
| `Data/` | `ApplicationDbContext`, seeder | `Negocio/AccesoDatos.cs` |
| `Data/Configuraciones/` | Un `IEntityTypeConfiguration<T>` por entidad | schema SQL manual |
| `Services/` | Lógica de negocio (interfaces + implementaciones) | `Negocio/*Negocio.cs` |
| `ViewModels/` | Modelos por vista | — |
| `Controllers/` | Controladores MVC | code-behind de `UserControls/` |
| `Views/` | Razor | designers de `UserControls/` |
| `Helpers/` | Constantes, mensajes | `Presentacion/Helpers/` |

### 4. Connection string

En `appsettings.json`:

```json
"ConnectionStrings": {
  "RecetarioMVC": "Server=.\\SQLEXPRESS;Database=RecetarioMVC;Integrated Security=True;TrustServerCertificate=True;"
}
```

Base **nueva** `RecetarioMVC` (code-first, se crea con la primera migración en la guía 03). La `RecetarioDB` original no se toca: sigue siendo del WinForms.

## Verificación

- `dotnet build Recetario-MVC/RecetarioMVC.csproj` → 0 errores.
- `dotnet run` → levanta la home del template MVC.
- La solución completa (proyectos .NET Framework incluidos) sigue compilando desde Visual Studio. `dotnet build` sobre el `.sln` no compila los proyectos WinForms (formato no-SDK): es esperado.

## Próximo paso

[Guía 03 — Modelo de datos](03-modelo-de-datos.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): crear proyecto base Recetario-MVC
- Proyecto ASP.NET Core MVC (.NET 9) agregado a la solución
- Paquetes EF Core 9 e Identity
- Estructura de carpetas y connection string a RecetarioMVC
```
