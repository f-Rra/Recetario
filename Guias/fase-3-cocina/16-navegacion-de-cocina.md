# Guía 16 — Navegación de cocina

> **Estado:** completada

## Objetivo

Ordenar lo que ve el rol Cocina en la sidebar y darle contenido propio a cada apartado. Al terminar, cocina tiene cuatro accesos con un propósito claro cada uno, sin duplicados.

## El problema

Después de la guía 15, el Home de Cocina redirige a la comandera. Como **Comandas** también abre la comandera, **Dashboard y Comandas llevaban exactamente a la misma pantalla**. Además el listado de comandas ya generadas quedaba escondido en un link dentro del encabezado.

## Estructura nueva

La sidebar se arma por rol, con las secciones ordenadas según lo que cada uno hace primero.

**Cocina**

```
OPERACIÓN
  Nueva comanda   → la comandera (pantalla de inicio)
  Comandas        → listado de las generadas, con filtros y reimpresión
CONSULTA
  Recetas         → catálogo con procedimiento completo
  Stock           → qué hay y qué falta (solo lectura)
```

**Admin**

```
GENERAL    Dashboard
GESTIÓN    Recetas · Ingredientes · Proveedores
OPERACIÓN  Nueva comanda · Comandas · Stock
SISTEMA    Responsables · Usuarios
```

Se elimina **Dashboard** para Cocina (era el duplicado de la comandera) y se separan las dos pantallas de comanda con nombres que no se confunden: *Nueva comanda* para armar, *Comandas* para el historial.

## Decisiones

- **Stock para Cocina es de solo lectura**: ve el listado con los estados crítico/bajo y el historial de movimientos, pero no puede registrar movimientos (sigue siendo exclusivo de Admin). Le sirve para planificar antes de armar la comanda, en vez de descubrir los faltantes recién al generar.
- **Recetas se mantiene** aunque el catálogo se repita en la comandera: es la única pantalla donde se ve el **procedimiento** de cada receta.
- El listado de comandas pasa de "un día" a **rango de fechas + filtro por receta**, con el PDF reimprimible desde cada fila.

## Piezas

| Archivo | Contenido |
|---|---|
| `Views/Shared/_Layout.cshtml` | Sidebar por rol con las secciones nuevas |
| `Controllers/StockController.cs` | `Index` e `Historial` para ambos roles; `RegistrarMovimiento` sigue solo Admin |
| `Views/Stock/Index.cshtml` | El botón de movimiento y el modal solo se muestran a Admin |
| `Services/IComandaService.cs` / `ComandaService.cs` | `ListarAsync(desde, hasta, idReceta)` reemplaza a `ListarPorFechaAsync` |
| `Controllers/ComandasController.cs` | `Index` con los tres filtros |
| `Views/Comandas/Index.cshtml` | Filtros, columna de fecha y botón de reimpresión por fila |

## Verificación

- Cocina ve cuatro ítems (Nueva comanda, Comandas, Recetas, Stock) y ningún Dashboard.
- Admin conserva todo, con Dashboard y las secciones ordenadas.
- Cocina entra a Stock y ve el listado, pero no el botón de registrar movimiento; el POST le da acceso denegado.
- El listado de comandas filtra por rango y por receta, y el PDF se descarga desde cualquier fila.

## Mensaje de commit

> Un solo commit con el código de esta guía + este .md actualizado a estado *completada*.

```
feat(mvc): reorganizar la navegación del rol Cocina
- Sidebar por rol y separación entre armar comanda e historial
- Stock de solo lectura para Cocina
- Listado de comandas con filtros por fecha y receta, y reimpresión
```
