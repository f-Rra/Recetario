# Guía 14 — Retiro del WinForms

> **Estado:** completada

## Objetivo

Cerrar la migración: el MVC ya cubre todas las funcionalidades del sistema viejo (fases 0–5), así que se retira la aplicación WinForms. Al terminar, la solución contiene solo el proyecto web y sus tests, y el README cuenta la historia completa del proyecto.

## Decisiones (acordadas con el usuario)

- **Proyectos WinForms → se borran del repo.** Se sacan del `.sln` y se eliminan las carpetas `Dominio/`, `Negocio/`, `Presentacion/` y `packages/` (NuGet estilo `packages.config`). La historia completa queda accesible en los commits anteriores; el repo de acá en adelante muestra solo lo vigente.
- **Scripts SQL → se conservan** en `scripts/`. Documentan el schema y la lógica original (SPs, triggers, vistas) que se reescribió como servicios C#: son la fuente de verdad contra la que se verificó cada reescritura (costeo, comandas, stock). Valen como registro de la migración.
- **`RecetarioDB` (base vieja)** no se toca desde acá: es un artefacto externo a este repo. El MVC usa su propia base `RecetarioMVC` code-first.

## Pasos

1. **Sacar los proyectos del `.sln`:**
   ```bash
   cd app/Recetario
   dotnet sln remove Dominio/Dominio.csproj
   dotnet sln remove Negocio/Negocio.csproj
   dotnet sln remove Presentacion/Presentacion.csproj
   ```
2. **Eliminar carpetas** `Dominio/`, `Negocio/`, `Presentacion/`, `packages/`.
3. **Verificar** que la solución queda solo con `RecetarioMVC` y `RecetarioMVC.Tests`, compila y los tests pasan.
4. **Reescribir el README** en la raíz: qué es el sistema, funcionalidades por rol, stack, cómo correrlo localmente, nota sobre la migración desde WinForms y el rol de `scripts/` y `Guias/`.

## Estado final del repo

```
Recetario/
  Guias/                    ← bitácora de la migración (00–14)
  scripts/                  ← schema y objetos SQL originales (documentación)
  app/Recetario/
    Recetario.sln           ← 2 proyectos: web + tests
    Recetario-MVC/          ← la aplicación
    Recetario-MVC.Tests/    ← 22 tests de servicios críticos
  README.md
  DER_Recetario_TPI.json / TPI-BD2-Recetario.pdf / Video_Demostrativo.mp4  ← material TPI original
```

## Verificación

- `dotnet sln list` → solo los 2 proyectos MVC.
- Las carpetas `Dominio/`, `Negocio/`, `Presentacion/`, `packages/` ya no existen.
- `dotnet build Recetario-MVC/RecetarioMVC.csproj` → 0 errores.
- `dotnet test` → 22/22 en verde.
- La app levanta y el login funciona (humo final).

## Cierre

Con esta guía termina la migración: 14 guías, 5 fases, el WinForms reemplazado por una app ASP.NET Core MVC con Identity, EF Core, la lógica de negocio en servicios testeados y PDFs con QuestPDF. Próximos pasos posibles (fuera de alcance de la migración): deploy, imagen en recetas, migración de datos productivos reales.

## Mensaje de commit

> Un solo commit con el retiro del WinForms + este .md + el README.

```
chore(mvc): retirar aplicación WinForms y cerrar la migración
- Baja de los proyectos Dominio, Negocio y Presentacion del .sln
- README reescrito con el estado final del proyecto
- Se conservan los scripts SQL originales como documentación
```
