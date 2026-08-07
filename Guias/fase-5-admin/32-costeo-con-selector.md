# Guía 32 — Costeo: selector de recetas y desglose con barras

> **Estado:** completada

## Objetivo

Sexto commit del rediseño de Admin. El costeo suma el **selector de recetas al
costado** y el desglose pasa a mostrar **cuánto pesa cada ingrediente** en el
total.

## Decisiones de diseño (cerradas con maquetas)

- **Selector a la izquierda** con todas las recetas y su clasificación. Antes
  había que volver al listado de recetas para costear otra; ahora se cambia de
  una. Reutiliza el `mdt-*` de ingredientes (guía 28), que se armó pensando en
  esto.
- **Sin id se abre la primera**, igual que ingredientes. Un id que ya no existe
  también cae en la primera en vez de romper.
- **Barras en el desglose**: cada ingrediente con su porcentaje del total. Los
  números solos no dejaban ver que las supremas son la mitad del costo de la
  ensalada; la barra sí.
- **Totales destacados a la derecha**: costo total en azul pleno y por porción
  en blanco, con Registrar debajo. Antes vivían en el pie de la tabla, donde se
  perdían.
- El **historial** queda abajo, con el PDF como píldora.
- El aviso de ingredientes sin precio deja de mandar al "botón $" de
  ingredientes, que ya no existe: los precios están en la ficha (guía 28).

## Piezas

| Archivo | Contenido |
|---|---|
| `ViewModels/CosteoViewModels.cs` | `Recetas` para el selector, `Participacion` para la barra |
| `Controllers/CostosController.cs` | Id opcional, selector en las tres acciones |
| `Views/Costos/Costear.cshtml` | Reescritura con selector, barras y totales |
| `wwwroot/css/site.css` | Lenguaje `cst-*` |

Sin cambios en `CosteoService`: el cálculo, que es la lógica crítica del
sistema (guía 10), no se tocó.

## Verificación

- El selector lista las 6 recetas; tocar una cambia el costeo y la URL.
- Entrar sin id abre la primera.
- Calcular 200 porciones de Ensalada César da $ 38.513,20, y la barra de las
  supremas ocupa cerca de la mitad.
- Las 6 recetas del demo se costean sin faltantes de precio.
- Registrar suma la fila al historial y el PDF se descarga.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): sumar el selector de recetas al costeo y desglosar con barras
- Se cambia de receta sin volver al listado
- Cada ingrediente muestra cuánto pesa en el costo total
- Los totales dejan el pie de la tabla y se destacan al costado
```
