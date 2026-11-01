# Recetario

[![CI](https://github.com/f-Rra/Recetario/actions/workflows/ci.yml/badge.svg)](https://github.com/f-Rra/Recetario/actions/workflows/ci.yml)
![.NET 9](https://img.shields.io/badge/.NET-9.0-blue)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-EF%20Core-CC2927)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED)

Sistema de recetas, costeo y comandas para cocina. Nació como trabajo práctico en WinForms y se migró a ASP.NET Core MVC.

## Funcionalidades

- **Costeo de recetas**: calcula el costo con el precio vigente de cada ingrediente, escalado a las porciones que se necesiten, y lo exporta a PDF.
- **Comandas con stock automático**: se elige la receta y los comensales, se pueden sustituir, sumar o sacar ingredientes, y el stock se descuenta solo. La comanda sale en PDF.
- **Stock auditado**: cada entrada, salida o ajuste queda como un movimiento con usuario y fecha, y el dashboard avisa qué ingredientes están por agotarse.
- **Ingredientes y proveedores** con el historial de precios de cada uno.
- **Dos perfiles**: administrador y cocina, con permisos distintos.

## Capturas

Una copia local con datos de demostración.

| | | | |
|---|---|---|---|
| <img src="docs/capturas/01-dashboard.png" width="200"> | <img src="docs/capturas/07-comandera.png" width="200"> | <img src="docs/capturas/04-costeo.png" width="200"> | <img src="docs/capturas/03-receta.png" width="200"> |
| Dashboard con métricas y actividad de stock | Comandera: escala a los comensales y descuenta stock | Costeo escalado por porciones | Receta con su procedimiento paso a paso |
| <img src="docs/capturas/02-ingredientes.png" width="200"> | <img src="docs/capturas/05-stock.png" width="200"> | <img src="docs/capturas/06-historial-comandas.png" width="200"> | |
| Ingredientes con stock e historial de precios | Movimientos de stock, filtrables y auditados | Historial de comandas | |

## Stack

.NET 9 · ASP.NET Core MVC · EF Core 9 (code-first, Fluent API) · SQL Server · ASP.NET Core Identity · Bootstrap 5 · QuestPDF · xUnit · Docker Compose · GitHub Actions.

## Modelo de datos

```mermaid
erDiagram
    CLASIFICACION ||--o{ RECETA : agrupa
    RECETA ||--o{ INGREDIENTE_RECETA : lleva
    INGREDIENTE ||--o{ INGREDIENTE_RECETA : "se usa en"
    RECETA ||--o{ PROCEDIMIENTO : "se hace en pasos"
    UNIDAD ||--o{ INGREDIENTE : "se mide en"
    INGREDIENTE ||--o{ PRECIO_INGREDIENTE : "tiene precios"
    PROVEEDOR ||--o{ PRECIO_INGREDIENTE : cotiza
    INGREDIENTE ||--o{ MOVIMIENTO_STOCK : mueve
    RECETA ||--o{ COMANDA : "se cocina en"
    PERSONA ||--o{ COMANDA : "es responsable de"
    COMANDA ||--o{ MODIFICACION : "se modifica con"
    RECETA ||--o{ COSTO : "se costea en"
    COSTO ||--|{ COSTO_DETALLE : detalla
    INGREDIENTE ||--o{ COSTO_DETALLE : "se cuenta en"

    RECETA {
        string Codigo
        string Nombre
        int PorcionesBase
    }
    INGREDIENTE {
        string Descripcion
        decimal StockActual "con token de concurrencia"
        decimal StockMinimo
        enum Deposito "Bodega, Camara"
    }
    INGREDIENTE_RECETA {
        decimal CantNeta
        decimal Rendimiento
        decimal CantBruta
    }
    PRECIO_INGREDIENTE {
        decimal Precio
        date FechaVigencia
    }
    COMANDA {
        date Fecha
        int Porciones
    }
    MODIFICACION {
        enum Tipo "Sustitucion, Adicion, Eliminacion"
        decimal Cantidad
    }
    MOVIMIENTO_STOCK {
        enum Tipo "Entrada, Salida, Ajuste"
        decimal Cantidad
        datetime Fecha
    }
    COSTO {
        int Porciones
        decimal CostoTotal
        decimal CostoUnitario
    }
```

Cada ingrediente de una receta lleva su cantidad neta, su rendimiento y su cantidad bruta. Cada costeo guarda su propio detalle con el costo unitario que se usó, así que un cambio de precio posterior no lo altera. Los usuarios del sistema son las tablas de ASP.NET Core Identity.

## Arquitectura

```mermaid
flowchart LR
    Usuario["Administrador o cocina<br/>(navegador)"] -->|cookie de Identity| MVC
    subgraph App["Aplicación ASP.NET Core"]
        MVC["Controladores MVC"] --> Serv
        Serv["Servicios<br/>costeo · comandas · stock · recetas"] --> PDF["PDF (QuestPDF)"]
    end
    Serv --> DB[("SQL Server")]
```

## Decisiones técnicas

- **Stock a prueba de comandas simultáneas.** Se valida dentro de la transacción y con un token de concurrencia: si dos comandas lo tocan a la vez, EF lo detecta y reintenta con el valor nuevo.
- **Un solo reloj con la hora de Argentina.** La aplicación pregunta la hora a un `TimeProvider` y no a `DateTime.Now`, porque un servidor en la nube corre en UTC, tres horas adelantado.
- **La lógica en servicios y los controladores delgados**, con interfaces e inyección de dependencias.
- **Sin claves en el repositorio.** La del administrador sale de user-secrets o de una variable de entorno; si falta, la aplicación no arranca.
- **Migrada con la referencia delante.** Los scripts SQL del sistema original se conservan y los servicios se verificaron contra ellos.

El porqué de cada una está en [`docs/decisiones-tecnicas.md`](docs/decisiones-tecnicas.md).

## Tests y CI

141 tests de xUnit sobre costeo, comandas, stock (incluida la concurrencia), dashboard y el manejo de la hora. Corren en cada push con GitHub Actions.

## Autor

- Estudiante de la Tecnicatura Universitaria en Programación - UTN FRGP
- Email: [facundo.rra@gmail.com](mailto:facundo.rra@gmail.com)
