# Guía 01 — Diseño visual

> **Fecha:** julio 2026
> **Estado:** vigente
> Complementa a la [guía 00](00-vision-y-decisiones.md). Existe una previsualización HTML interactiva del sistema de diseño (artifact "Recetario MVC — Sistema de diseño").

## 1. Identidad

La identidad visual se basa en la marca de **Grupo Southex** (facility management & food services). No se hereda nada del WinForms actual, que usa colores de sistema.

### Paleta

| Token | Hex | Uso |
|---|---|---|
| `--sx-azul` | `#1078B0` | Primario: sidebar, botones principales, links, encabezados de tabla |
| `--sx-azul-oscuro` | `#0C5A85` | Hover/active del primario, texto sobre fondos celestes |
| `--sx-celeste` | `#68A8D0` | Secundario: hovers suaves, bordes de foco, gráficos, badges informativos |
| `--sx-celeste-claro` | `#EAF3F9` | Fondos de fila seleccionada, cards de métricas, alerts info |
| `--sx-rojo` | `#E01820` | Acento: acciones destructivas, alertas de error, badges de stock crítico |
| `--sx-blanco` | `#FFFFFF` | Fondo de contenido y cards |
| `--sx-gris-fondo` | `#F5F7F9` | Fondo general de la app (detrás de las cards) |
| `--sx-gris-texto` | `#2B3A45` | Texto principal (no negro puro) |
| `--sx-gris-suave` | `#6C7A85` | Texto secundario, labels, placeholders |

**Reglas:**
- El rojo es **solo** para destrucción/peligro/alertas. Nunca para decoración ni acciones neutras.
- Un solo botón primario (azul) por pantalla/card; el resto son `outline` o `link`.
- Estados de stock: normal (sin color), bajo (badge celeste), crítico (badge rojo).

### Tipografía

- **UI**: `Segoe UI` con fallback `system-ui, sans-serif` — nativa de Windows (los usuarios son desktop), sin webfonts que cargar.
- **Números tabulares** en columnas de costos/cantidades: `font-variant-numeric: tabular-nums`.
- Escala: base 16px; títulos de página 24px/600; títulos de card 18px/600; texto de tabla 14px.

## 2. Layout

```
┌──────────┬──────────────────────────────────────┐
│          │  Topbar: título de página + usuario   │
│ Sidebar  ├──────────────────────────────────────┤
│ (azul)   │                                      │
│          │  Contenido: cards blancas sobre      │
│ 240px    │  fondo gris #F5F7F9                  │
│ fija     │                                      │
└──────────┴──────────────────────────────────────┘
```

- **Sidebar** fija de 240px, fondo azul `#1078B0`, texto blanco. Logo/nombre arriba, navegación por módulo con ícono + label, usuario y logout abajo.
- Ítem activo: fondo blanco semitransparente + borde izquierdo blanco de 3px.
- La navegación visible depende del rol: Admin ve todo; Cocina ve solo Dashboard Cocina y Recetas (consulta).
- **Topbar** blanca con el título de la página actual y el nombre del usuario logueado.
- **Contenido**: cards blancas con `border-radius: 8px` y sombra sutil, sobre fondo gris. Sin bordes duros.
- Desktop-only: ancho mínimo soportado 1280px. No se invierte en responsive (decisión de guía 00).

## 3. Componentes

### Botones
- Primario: fondo azul, texto blanco. Hover: `--sx-azul-oscuro`.
- Secundario: `outline` azul.
- Destructivo: fondo rojo, solo en confirmaciones de borrado.
- Íconos: Bootstrap Icons (CDN o local), siempre acompañando el texto en acciones principales.

### Tablas
- Encabezado con fondo `--sx-celeste-claro`, texto azul oscuro, sin zebra striping.
- Hover de fila: `--sx-celeste-claro`.
- Acciones por fila a la derecha (íconos editar/eliminar), no botones anchos.
- Toda tabla de listado lleva búsqueda/filtro arriba a la izquierda y botón "+ Nuevo" arriba a la derecha.

### Formularios
- Labels arriba del campo (no flotantes), asterisco rojo en obligatorios.
- Validación: borde rojo + mensaje debajo (jQuery Validation Unobtrusive, el default de MVC).
- Foco: borde `--sx-celeste` con sombra suave.

### Cards de métricas (dashboards)
- Card blanca con número grande (32px/700, azul), label gris arriba, ícono celeste a la derecha.
- Métricas de alerta (ej. stock crítico) usan el número en rojo.

### Feedback
- Operación exitosa: toast/alert verde Bootstrap estándar (el verde de Bootstrap se mantiene para success).
- Errores de negocio: alert con borde rojo dentro de la card.
- Confirmación de borrado: modal Bootstrap con botón rojo.

## 4. Implementación

- Variables CSS en `wwwroot/css/site.css` bajo `:root` con los tokens de la paleta (prefijo `--sx-`).
- Se sobreescriben las variables de Bootstrap 5.3 (`--bs-primary`, `--bs-danger`, etc.) para que los componentes hereden la paleta sin recompilar Sass.
- Un único `site.css` custom; nada de estilos inline en las vistas.
- Bootstrap y Bootstrap Icons servidos localmente desde `wwwroot/lib/` (la app corre en red local, sin depender de CDN).

## Mensaje de commit

```
docs: agregar guía de diseño visual
- Paleta Southex con tokens CSS y reglas de uso
- Tipografía, layout sidebar y especificación de componentes
```
