# Guía 24 — Redondeo de cantidades

> **Estado:** completada

## Objetivo

Que la comanda pida cantidades que se puedan usar. Al escalar una receta a los
comensales aparecen números como **1,51 u de ajo**, **6,8 g de sal** o
**1,89 kg de supremas**: nadie saca 1,51 cabezas de ajo del depósito ni pesa
décimas de gramo.

El redondeo es **contable, no cosmético**: el cocinero saca 2 cabezas, así que
2 es lo que tiene que descontarse del stock, guardarse en la comanda e
imprimirse en el PDF. Los tres números coinciden.

## La regla

| Unidad | Se redondea a | Ejemplo |
|---|---|---|
| **u** | entero | 1,51 u → 2 u |
| **g / ml** | entero | 6,8 g → 7 g |
| **kg / L** | medio (0,5) | 1,89 kg → 2 kg · 3,2 kg → 3 kg |

- Siempre **al más cercano**, con el empate hacia arriba (1,5 u → 2 u).
- **Nunca a cero**: si la cantidad es mayor que cero y el redondeo la anularía,
  queda en el paso mínimo de su unidad (1 u, 1 g, 0,5 kg). Si no, una comanda
  chica haría desaparecer un ingrediente de la receta.

### Por qué medio kilo y no entero

Abajo de 1 kg la cantidad ya se muestra en gramos, así que los kilos viven casi
siempre entre 1 y 5 — justo donde el entero hace más daño: 1,4 kg → 1 kg es un
29% menos, tres porciones que no salen. El medio kilo acota el error a 250 g,
da el mismo resultado en los casos frecuentes (1,89 → 2, 3,2 → 3) y es como se
habla en la cocina.

### La unidad de la regla es la que se lee

El redondeo se aplica sobre la unidad en la que la cantidad **se muestra**, no
sobre la que está guardada. La lechuga se guarda en gramos: 3200 g se leen como
3,2 kg, redondea a 3 kg y se descuentan 3000 g. Por eso `FormatoCantidad`
expone su conversión y `RedondeoCocina` la reutiliza: la regla y la pantalla no
pueden discrepar.

## Dónde se aplica

Sobre la **comanda ya escalada y con las modificaciones aplicadas**, una sola
vez, en `CalcularEfectivosPorRecetaAsync`. De ahí salen a la vez el descuento de
stock, las modificaciones persistidas y las secciones del PDF.

También lo usan, con la misma función:

- La **previsualización** del catálogo: lo que se ve al armar es lo que se va a
  descontar.
- El **aviso de faltantes**: si hacen falta 2 ajos, tener 1,51 no alcanza.
- El **detalle de una comanda ya generada**, que reescala desde la receta.

**Lo que no se redondea:** la receta en sí. Los 0,8 g de sal por 10 porciones
siguen exactos en la definición y en el costeo; el redondeo actúa recién cuando
la receta se lleva a una comanda. Tampoco el stock ni sus movimientos manuales,
que son cantidades medidas de verdad.

## Piezas

| Archivo | Contenido |
|---|---|
| `Helpers/RedondeoCocina.cs` | La regla |
| `Helpers/FormatoCantidad.cs` | `UnidadDeLectura` pasa a ser pública |
| `Services/ComandaService.cs` | Redondeo en efectivos, preview y detalle |
| `Recetario-MVC.Tests/RedondeoCocinaTests.cs` | La regla, unidad por unidad |
| `Recetario-MVC.Tests/ComandaServiceTests.cs` | Que se descuente lo redondeado |

## Verificación

- Una comanda de 85 comensales de Ensalada César pide 2 u de ajo, 7 g de sal y
  2 kg de supremas, y eso es lo que descuenta el stock y sale en el PDF.
- El detalle de esa comanda muestra los mismos números que el PDF.
- La previsualización del catálogo coincide con lo que después se descuenta.
- Las cantidades de la receta en la pantalla de edición siguen sin redondear.
- `dotnet test` en verde.

## Mensaje de commit

```
feat(mvc): redondear las cantidades de la comanda a medidas usables
- Unidades y gramos enteros, kilos y litros al medio
- Se descuenta y se imprime lo mismo que se muestra
```
