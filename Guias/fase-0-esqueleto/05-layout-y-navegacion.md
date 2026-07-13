# Guía 05 — Layout y navegación

> **Estado:** completada

## Objetivo

Reemplazar el layout del template por el diseño de la [guía 01](01-diseno-visual.md): sidebar azul fija con navegación filtrada por rol, topbar con el título de página y el usuario logueado (con logout), y contenido en cards sobre fondo gris. Al terminar, toda pantalla interna se ve "Southex" y se navega sin escribir URLs.

## Piezas

| Archivo | Contenido |
|---|---|
| `wwwroot/css/site.css` | Se reescribe: tokens `--sx-*` de la paleta, override de variables Bootstrap (`--bs-primary`, etc.), estilos de sidebar/topbar/cards/tablas |
| `wwwroot/lib/bootstrap-icons/` | Bootstrap Icons servido local (CSS + fuentes), sin CDN: la app corre en red local |
| `Views/Shared/_Layout.cshtml` | Se reescribe: sidebar + topbar + `RenderBody` en área de contenido |
| `Views/Shared/_Alertas.cshtml` | Parcial que muestra `TempData["Exito"]`/`TempData["Error"]`; se renderiza en el layout para todas las páginas |
| `Views/Home/Index.cshtml` | Placeholder "Dashboard" con el layout nuevo (el contenido real es la guía 06) |
| `Controllers/HomeController.cs` + vistas | Limpieza del template: se elimina `Privacy` |
| `Views/Usuarios/Index.cshtml` | Se quitan las alertas locales (ahora las muestra el layout) |

## Estructura de la sidebar

```
Recetario / GRUPO SOUTHEX
GENERAL
  Dashboard        → Home           (Admin y Cocina)
  Recetas          → deshabilitado  (Admin y Cocina; se habilita en guía 09)
  Ingredientes     → deshabilitado  (solo Admin; guía 07)
  Proveedores      → deshabilitado  (solo Admin; guía 08)
OPERACIÓN
  Comandas         → deshabilitado  (Admin y Cocina; guía 11)
  Stock            → deshabilitado  (solo Admin; guía 12)
SISTEMA
  Usuarios         → /Usuarios      (solo Admin)
─────────────────────────
  {Nombre del usuario}  ·  Salir (POST Logout)
```

- Los módulos que todavía no existen aparecen **deshabilitados** (gris, sin link) para mostrar la estructura final sin generar 404; cada guía de módulo habilita su ítem.
- Ítem activo: fondo blanco semitransparente + borde izquierdo blanco 3px (según guía 01), detectado por el controller actual.
- La topbar muestra `ViewData["Title"]` a la izquierda y "Conectado como **{nombre}** ({rol})" a la derecha.
- El logout es un `<form method="post">` con antiforgery (no un link GET).

## Detalles de implementación

- `site.css` define los tokens en `:root` y pisa las variables de Bootstrap 5.3 para que `btn-primary`, `badge`, etc. hereden la paleta sin recompilar Sass.
- El layout obtiene el usuario con `@inject UserManager<ApplicationUser>` para mostrar el nombre completo.
- El login (`Layout = null`) no se toca.
- Desktop-only (guía 00): la sidebar es fija, sin colapsado responsive.

## Verificación

- Login como admin → sidebar azul con las 3 secciones y "Usuarios" visible; topbar con "Conectado como Administrador Sistema (Admin)".
- Navegar a Usuarios desde la sidebar; crear/desactivar muestran la alerta arriba del contenido (desde el layout).
- Botón "Salir" → vuelve al login.
- Login como cocina (incógnito) → la sidebar NO muestra Ingredientes/Proveedores/Stock/Usuarios.
- Los ítems deshabilitados no navegan.

## Próximo paso

[Guía 06 — Dashboard Admin](06-dashboard-admin.md).

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): agregar layout con sidebar y navegación por rol
- Sidebar Southex fija con secciones y módulos deshabilitados
- Topbar con usuario logueado y logout
- site.css con tokens de la paleta y override de Bootstrap
```
