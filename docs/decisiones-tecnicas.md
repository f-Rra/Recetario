# Decisiones técnicas

## Stock a prueba de comandas simultáneas

Dos personas generando una comanda a la vez no pueden dejar el stock mal. El stock se valida adentro de la transacción y lleva un token de concurrencia: si alguien lo cambia entre la lectura y el guardado, EF lo detecta y la operación se reintenta con el valor nuevo. Los tests lo simulan con un segundo contexto que cambia el stock justo antes del guardado.

## Un solo reloj con la hora de Argentina

Un servidor en la nube corre en UTC, tres horas adelantado. Toda la aplicación pregunta la hora a un `TimeProvider` en lugar de usar `DateTime.Now`, y los tests lo reemplazan por un reloj falso para simular, por ejemplo, las 23:30 de Argentina cuando en UTC ya es el día siguiente.

## La lógica en servicios

La lógica de negocio vive en `Services/`, con interfaces e inyección de dependencias; los controladores son delgados. Los servicios críticos (costeo, comandas y stock) están cubiertos por **141 tests** que corren en cada push y que, entre otras cosas, verifican paridad con el sistema original.

## Sin claves en el repositorio

La clave del administrador inicial sale de `DefaultAdmin:Password` (user-secrets o variable de entorno) y, si falta, la aplicación no arranca.

## Una base de demostración reproducible

Docker Compose levanta la aplicación con su propia base de SQL Server, y un script de datos de ejemplo (`scripts/Datos_Demo_MVC.sql`) la llena con 27 ingredientes, 6 recetas con su procedimiento y precios de ejemplo.

## La migración desde WinForms

El proyecto nació como TPI académico en WinForms (.NET Framework, 3 capas, ADO.NET y stored procedures). La migración a ASP.NET Core MVC se hizo en fases (esqueleto, CRUDs, núcleo y costeo, cocina, soporte y cierre), documentadas en [`Guias/`](../Guias/).

Además de trasladar la funcionalidad, se corrigieron limitaciones del diseño original: historial real de precios, escalado de costeo por porciones, bloqueo de costeos incompletos y trazabilidad completa del stock mediante movimientos auditados.

`scripts/` conserva la base de datos y la lógica del sistema original. Se mantiene como documentación: fue la referencia contra la que se verificó cada servicio reescrito en C# (por ejemplo, el costeo replica la selección de precio vigente del `sp_CalcularCostoReceta`, y el consumo de stock replica `trg_ConsumirStockComanda`).

## Estructura del proyecto

```
app/Recetario/
├─ Recetario-MVC/          la aplicación: controladores, servicios, datos, vistas
└─ Recetario-MVC.Tests/    los tests de xUnit
docs/                      capturas y documentación
Guias/                     bitácora de la migración, una guía por commit
scripts/                   la base y la lógica del sistema original, y los datos de demostración
docker-compose.yml         la aplicación y su base, con un comando
```
