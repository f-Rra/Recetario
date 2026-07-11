# Guía 00 — Visión y decisiones de la migración

> **Fecha:** julio 2026
> **Estado:** vigente

## 1. Contexto

El Recetario actual es una aplicación de escritorio WinForms (.NET Framework 4.8.1) en 3 capas (`Dominio` / `Negocio` / `Presentacion`), con acceso a datos ADO.NET puro contra SQL Server (stored procedures y vistas). Nació como TPI académico y funciona, pero su arquitectura limita el uso real: requiere instalación por máquina, no tiene seguridad de contraseñas (viajan en texto plano al SP) y la UI de escritorio no escala a nuevos usuarios.

Este documento define la migración a una aplicación web ASP.NET Core MVC, siguiendo el proceso ya probado en [Control-Almuerzos-MVC](https://github.com/f-Rra/Control-Almuerzos-MVC).

## 2. Objetivos

1. **Uso real**: que la empresa pueda operar el recetario desde el navegador, con seguridad y validaciones de nivel productivo.
2. **Portfolio**: código y proceso prolijos, documentados en guías, que muestren buenas prácticas de arquitectura y migración.

## 3. Decisiones

| Tema | Decisión | Justificación |
|---|---|---|
| Ubicación | Proyecto `Recetario-MVC` dentro de `app/Recetario`, reemplaza gradualmente al WinForms | Mismo repo, historia unificada; el WinForms sigue funcionando hasta el final |
| Framework | ASP.NET Core MVC sobre .NET 9 | Consistencia con Control-Almuerzos-MVC; LTS-adjacent, SDK ya instalado |
| ORM | EF Core 9 con Fluent API (`IEntityTypeConfiguration<T>` por entidad) | Consistencia entre proyectos; LINQ tipado reemplaza SQL embebido |
| Base de datos | Nueva `RecetarioMVC` **code-first** + migrations + seeder | Libertad para corregir el schema viejo; la `RecetarioDB` original queda intacta para el WinForms durante la convivencia |
| SPs y vistas | Se reescriben como servicios C#/LINQ (`sp_CalcularCostoReceta`, `vw_Comanda`, `vw_CostoReceta`) | Lógica testeable y versionada en el repo; ver guía 10 |
| Autenticación | ASP.NET Core Identity, roles `Admin` y `Cocina`, **sin registro público**: solo el Admin da de alta usuarios | Modelo actual de 2 roles; resuelve el problema de contraseñas planas (no se migran usuarios viejos) |
| UI | Bootstrap 5, sidebar lateral fija, desktop-only | Ver guía 01 — Diseño visual |
| Identidad visual | Paleta del logo de Grupo Southex: azul `#1078B0`, rojo `#E01820`, celeste `#68A8D0`, blanco | Alineación con la marca de la empresa |
| Alcance | **Mejorar sobre la marcha**: cada módulo se rediseña al migrarlo (flujos, validaciones, búsquedas) | La web permite mejores patrones que la grilla WinForms; no vale la pena replicar limitaciones |
| PDFs | QuestPDF reemplaza a iTextSharp | Licencia community gratuita, API moderna, PDFs consistentes generados en servidor |
| Testing | xUnit solo en servicios críticos: **costeo, stock, comandas** | Es la lógica que no puede fallar en uso real; el resto se valida manualmente contra el WinForms |
| Git | Commits convencionales (`feat`/`fix`/`docs`/`refactor`) directo a `main` | Historia lineal, mismo flujo que hasta ahora |
| Idioma | Código en español (entidades, servicios, vistas) | Consistencia con el dominio y el código existente |
| Deploy | Local por ahora (Kestrel + SQL Express) | El destino final se decide al terminar; sin costos durante el desarrollo |

## 4. Fases

| Fase | Contenido | Guías |
|---|---|---|
| 0 — Esqueleto | Proyecto, modelo de datos, Identity, layout, **Login + Dashboard Admin** | 02–06 |
| 1 — CRUD simple | Ingredientes, Proveedores | 07–08 |
| 2 — Núcleo | Recetas + Costeo (reescritura de `sp_CalcularCostoReceta`, con tests) | 09–10 |
| 3 — Cocina | Comandas, dashboard Cocina | 11 |
| 4 — Soporte | Stock/movimientos, clasificaciones, unidades, historial de modificaciones | 12 |
| 5 — Cierre | PDFs con QuestPDF, retiro del WinForms del `.sln` | 13–14 |

**Regla de convivencia:** el WinForms permanece funcional en el `.sln` hasta la fase 5. Cada fase termina con la app web corriendo y lo migrado hasta ahí usable de punta a punta.

## 5. Índice de guías

Las guías se organizan en carpetas por fase. **Cada guía es una unidad de trabajo y un commit**: primero se escribe como plan (estado *pendiente*), se implementa, y se commitea el código junto con la guía actualizada (estado *completada*).

```
Guias/
  fase-0-esqueleto/
    00-vision-y-decisiones.md   ← este documento
    01-diseno-visual.md         ← paleta, tipografía, layout, componentes
    02-setup-proyecto.md        ← creación de Recetario-MVC, estructura, .sln
    03-modelo-de-datos.md       ← entidades, Fluent API, migración, seeder
    04-identity-y-roles.md      ← login, roles, gestión de usuarios
    05-layout-y-navegacion.md   ← sidebar, _Layout, parciales
    06-dashboard-admin.md
  fase-1-crud/
    07-ingredientes.md
    08-proveedores.md
  fase-2-nucleo/
    09-recetas.md
    10-costeo.md                ← reescritura de sp_CalcularCostoReceta + tests
  fase-3-cocina/
    11-comandas-cocina.md
  fase-4-soporte/
    12-stock-y-movimientos.md
  fase-5-cierre/
    13-pdfs-questpdf.md
    14-retiro-del-winforms.md
```

## 6. Riesgos conocidos

1. **Costeo**: `sp_CalcularCostoReceta` usa `ROW_NUMBER` para elegir el precio vigente por ingrediente. Antes de reescribirlo, capturar casos de prueba con resultados reales del sistema actual (recetas conocidas → costo esperado) y verificar paridad exacta con tests.
2. **Datos productivos**: la DB nueva arranca vacía + seed. Si al momento del switch hay datos reales cargados en `RecetarioDB`, se necesitará un script de migración de datos puntual (se define en la fase 5).
3. **Mejorar sobre la marcha** dificulta comparar contra el original: cada guía de módulo debe listar explícitamente qué se mantiene igual y qué cambia.

## Mensaje de commit

```
docs: agregar guía de visión y decisiones de la migración a MVC
- Objetivos, decisiones con justificación y fases
- Índice de guías y riesgos conocidos
```
