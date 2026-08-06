# Guía 27 — Sidebar plano y modales centrados

> **Estado:** completada

## Objetivo

Primer commit del rediseño de Admin (maquetas de agosto 2026). Dos cambios
transversales y chicos, para dejar el terreno listo antes de tocar pantalla por
pantalla.

El rediseño de Admin se commitea **de a una pantalla**, en este orden:

| # | Commit | Guía |
|---|---|---|
| 1 | Sidebar y modales | esta |
| 2 | Ingredientes con ficha y precios | 28 |
| 3 | Proveedores | 29 |
| 4 | Responsables | 30 |
| 5 | Usuarios | 31 |
| 6 | Costeo | 32 |
| 7 | Editar receta | 33 |
| 8 | Dashboard | 34 |

## Sidebar

- **Se van los rótulos de sección** (General, Gestión, Operación, Sistema). Con
  ocho accesos agregaban ruido sin ayudar a encontrar nada, y el menú de Cocina
  —que nunca los tuvo— se lee mejor.
- **Se va "Nueva comanda"** del menú de Admin: armar comandas es trabajo de
  cocina. La ruta sigue existiendo y accesible para los dos roles; lo que
  desaparece es el acceso directo.

Queda una lista plana de ocho: Dashboard, Recetas, Ingredientes, Proveedores,
Comandas, Stock, Responsables, Usuarios.

## Modales centrados

Bootstrap apoya los modales cerca del borde superior salvo que se le pida lo
contrario. Con `modal-dialog-centered` quedan en el medio de la pantalla, que
es donde se los busca con la vista.

Son **siete**, no cuatro: además de los de cocina, cada listado de Admin tiene
su modal de eliminación.

| Pantalla | Modal |
|---|---|
| Comandera | Generar comanda · Modificar receta |
| Listado de recetas | Eliminar receta |
| Stock | Registrar movimiento |
| Ingredientes | Eliminar ingrediente |
| Proveedores | Eliminar proveedor |
| Responsables | Eliminar responsable |

En el de generar comanda, `modal-dialog-centered` convive con
`modal-dialog-scrollable`: con muchos faltantes el cuerpo scrollea y el pie
queda a la vista, ahora desde el centro.

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Shared/_Layout.cshtml` | Menú de Admin plano, sin Nueva comanda |
| `Views/Comandas/Comandera.cshtml` | Dos modales centrados |
| `Views/Recetas/Index.cshtml` · `Stock/Index.cshtml` | Modal centrado |
| `Views/Ingredientes/Index.cshtml` · `Proveedores/Index.cshtml` · `Responsables/Index.cshtml` | Modal centrado |

Sin cambios de lógica: `dotnet test` debe seguir en verde sin tocar nada.

## Verificación

- Como Admin, el menú lateral no tiene rótulos de sección ni "Nueva comanda", y
  el ítem activo se sigue marcando al navegar.
- Como Cocina, el menú queda igual que antes.
- Los siete modales abren centrados verticalmente.
- El de generar comanda, con faltantes, sigue scrolleando por dentro.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): aplanar el menú de admin y centrar los modales
- Menú sin rótulos de sección ni acceso a nueva comanda
- Los siete modales quedan centrados en la pantalla
```
