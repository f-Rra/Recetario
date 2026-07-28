/*
    Datos de demostración para RecetarioMVC
    --------------------------------------------------------------
    Port de Datos_Prueba.sql (sistema WinForms) al schema nuevo:
      · TiposMovimiento / TiposModificacion son enums, no tablas
      · Usuarios los maneja Identity (el seeder crea admin@recetario.local)
      · PrecioxIngrediente pasó a PreciosIngrediente con clave propia
      · El stock inicial de cada ingrediente queda auditado como movimiento

    Requiere que la app haya corrido al menos una vez (crea catálogos y admin).
    Borra y recarga los datos operativos: no ejecutar sobre datos reales.
*/

USE RecetarioMVC;
GO

-- ---------- Limpieza (orden seguro por claves foráneas) ----------
DELETE FROM Modificaciones;
DELETE FROM Comandas;
DELETE FROM CostosDetalle;
DELETE FROM Costos;
DELETE FROM MovimientosStock;
DELETE FROM IngredientesReceta;
DELETE FROM Procedimientos;
DELETE FROM PreciosIngrediente;
DELETE FROM Recetas;
DELETE FROM Ingredientes;
DELETE FROM Proveedores;
DELETE FROM Personas;
GO

-- ---------- Responsables de cocina ----------
SET IDENTITY_INSERT Personas ON;
INSERT INTO Personas (IdPersona, Nombre, Apellido, Email, Telefono, IdClasificacion) VALUES
(1, N'Carlos',  N'Gómez',     NULL,                              NULL, 1),
(2, N'María',   N'López',     NULL,                              NULL, 2),
(3, N'Pedro',   N'Fernández', NULL,                              NULL, 3),
(4, N'Laura',   N'Sánchez',   NULL,                              NULL, 5),
(5, N'Jorge',   N'Torres',    NULL,                              NULL, 6),
(6, N'Facundo', N'Herrera',   N'facundo.herrera@recetario.com',  NULL, 2),
(7, N'Ana',     N'Martínez',  N'ana.martinez@recetario.com',     NULL, NULL);
SET IDENTITY_INSERT Personas OFF;
GO

-- ---------- Proveedores ----------
SET IDENTITY_INSERT Proveedores ON;
INSERT INTO Proveedores (IdProveedor, Nombre, Contacto, Telefono, Email, Direccion) VALUES
(1, N'Distribuidor Central', N'Roberto Díaz',   N'011-4567-8901', N'ventas@distcentral.com',     N'Av. Belgrano 1200, CABA'),
(2, N'Verduras del Sur',     N'Graciela Pérez', N'011-3456-7890', N'info@verdurasdelsur.com',    N'Mercado Central, Puesto 45'),
(3, N'Lácteos Frescos',      N'Miguel Torres',  N'011-5678-9012', N'pedidos@lacteosfrescos.com', N'Ruta 8 km 32, GBA');
SET IDENTITY_INSERT Proveedores OFF;
GO

-- ---------- Ingredientes ----------
SET IDENTITY_INSERT Ingredientes ON;
INSERT INTO Ingredientes (IdIngrediente, Codigo, Descripcion, IdUnidad, StockActual, StockMinimo) VALUES
( 1, 'ING001', N'Lechuga',            1,    2.5000,   2.0000),
( 2, 'ING002', N'Crutones',           1,    0.5000,   1.0000),
( 3, 'ING003', N'Supremas de pollo',  1,   15.0000,  10.0000),
( 4, 'ING004', N'Queso parmesano',    1,    0.3000,   0.5000),
( 5, 'ING005', N'Leche',              4, 1500.0000, 500.0000),
( 6, 'ING006', N'Ajo',                5,    8.0000,   5.0000),
( 7, 'ING007', N'Aceite de girasol',  4,  800.0000, 500.0000),
( 8, 'ING008', N'Sal fina',           2,  500.0000, 100.0000),
( 9, 'ING009', N'Pimienta negra',     2,   80.0000, 100.0000),
(10, 'ING010', N'Papas',              1,    8.0000,   5.0000),
(11, 'ING011', N'Puerros',            1,    1.5000,   2.0000),
(12, 'ING012', N'Cebolla',            1,    3.0000,   1.0000),
(13, 'ING013', N'Caldo de verduras',  2,  200.0000, 100.0000),
(14, 'ING014', N'Aceite de oliva',    4,  500.0000, 200.0000),
(15, 'ING015', N'Harina 0000',        1,    5.0000,   2.0000),
(16, 'ING016', N'Manteca',            1,    0.8000,   1.0000),
(17, 'ING017', N'Leche en polvo',     1,    3.0000,   2.0000),
(18, 'ING018', N'Nuez moscada',       2,   50.0000,  20.0000),
(19, 'ING019', N'Morrón',             1,    1.5000,   1.0000),
(20, 'ING020', N'Apio',               1,    2.0000,   1.0000),
(21, 'ING021', N'Arroz largo fino',   1,    4.0000,   2.0000),
(22, 'ING022', N'Manzanas',           1,    8.0000,   5.0000),
(23, 'ING023', N'Azúcar',             1,    3.0000,   1.0000),
(24, 'ING024', N'Huevos',             5,   30.0000,  12.0000),
(25, 'ING025', N'Atún en lata',       2,  300.0000, 200.0000),
(26, 'ING026', N'Zanahorias',         1,    3.0000,   1.0000),
(27, 'ING027', N'Perejil',            2,   40.0000,  50.0000);
SET IDENTITY_INSERT Ingredientes OFF;
GO

-- ---------- Recetas ----------
SET IDENTITY_INSERT Recetas ON;
INSERT INTO Recetas (IdReceta, Codigo, Nombre, IdClasificacion, PorcionesBase, Activo, Imagen) VALUES
(1, 'REC001', N'Ensalada César',           5,  10, 1, NULL),
(2, 'REC002', N'Sopa de papas y puerros',  1,   1, 1, NULL),
(3, 'REC003', N'Salsa blanca',             6,  10, 1, NULL),
(4, 'REC004', N'Pollo al horno con arroz', 2,  10, 1, NULL),
(5, 'REC005', N'Tarta de manzana',         3, 100, 1, NULL),
(6, 'REC006', N'Mayonesa de zanahorias',   4,   1, 1, NULL);
SET IDENTITY_INSERT Recetas OFF;
GO

-- ---------- Precios por proveedor ----------
INSERT INTO PreciosIngrediente (IdIngrediente, IdProveedor, Precio, FechaVigencia) VALUES
( 1, 2,   800.0000, '2026-05-01'),
( 2, 1,  1200.0000, '2026-05-01'),
( 3, 1,  4500.0000, '2026-05-01'),
( 4, 3,  6500.0000, '2026-05-01'),
( 5, 3,     2.8000, '2026-05-01'),
( 6, 2,   400.0000, '2026-05-01'),
( 7, 1,     3.2000, '2026-05-01'),
( 8, 1,     0.8000, '2026-05-01'),
( 9, 1,    12.0000, '2026-05-01'),
(10, 2,   350.0000, '2026-05-01'),
(11, 2,   650.0000, '2026-05-01'),
(12, 2,   280.0000, '2026-05-01'),
(13, 1,     4.0000, '2026-05-01'),
(14, 1,     2.2000, '2026-05-01'),
(15, 1,   380.0000, '2026-05-01'),
(16, 3,  1400.0000, '2026-05-01'),
(17, 3,  8000.0000, '2026-05-01'),
(18, 1,    80.0000, '2026-05-01'),
(19, 2,  1200.0000, '2026-05-01'),
(20, 2,   500.0000, '2026-05-01'),
(21, 1,   290.0000, '2026-05-01'),
(22, 2,   750.0000, '2026-05-01'),
(23, 1,   250.0000, '2026-05-01'),
(24, 1,   180.0000, '2026-05-01'),
(25, 1,     8.0000, '2026-05-01'),
(26, 2,   350.0000, '2026-05-01'),
(27, 2,     0.6000, '2026-05-01');
GO

-- ---------- Ingredientes de cada receta ----------
INSERT INTO IngredientesReceta (IdReceta, IdIngrediente, CantNeta, Rendimiento, CantBruta, IdUnidad) VALUES
(1,  1,   0.3200,  85.00,   0.3765, 1),
(1,  2,   0.0400, 100.00,   0.0400, 1),
(1,  3,   0.2000,  90.00,   0.2222, 1),
(1,  4,   0.0400, 100.00,   0.0400, 1),
(1,  5,  40.0000, 100.00,  40.0000, 4),
(1,  6,   0.1600,  90.00,   0.1778, 5),
(1,  7,  40.0000, 100.00,  40.0000, 4),
(1,  8,   0.8000, 100.00,   0.8000, 2),
(1,  9,   0.4000, 100.00,   0.4000, 2),
(2, 10,   0.5000,  80.00,   0.6250, 1),
(2, 11,   0.3000,  85.00,   0.3529, 1),
(2, 12,   0.0500,  85.00,   0.0588, 1),
(2, 13,   5.0000, 100.00,   5.0000, 2),
(2, 14,   5.0000, 100.00,   5.0000, 4),
(2,  8,  10.0000, 100.00,  10.0000, 2),
(2,  9,   2.0000, 100.00,   2.0000, 2),
(3, 17,   1.0000, 100.00,   1.0000, 1),
(3, 16,   0.1000, 100.00,   0.1000, 1),
(3, 15,   0.1000, 100.00,   0.1000, 1),
(3,  8,  50.0000, 100.00,  50.0000, 2),
(3,  9,  10.0000, 100.00,  10.0000, 2),
(3, 18,  10.0000, 100.00,  10.0000, 2),
(4,  3,   2.0000,  90.00,   2.2222, 1),
(4, 12,   0.5000,  85.00,   0.5882, 1),
(4, 19,   0.2000,  80.00,   0.2500, 1),
(4, 11,   0.1000,  85.00,   0.1176, 1),
(4, 20,   0.1000,  85.00,   0.1176, 1),
(4,  8,  20.0000, 100.00,  20.0000, 2),
(4,  9,   2.0000, 100.00,   2.0000, 2),
(4, 14,  10.0000, 100.00,  10.0000, 4),
(4, 21,   1.0000, 100.00,   1.0000, 1),
(4, 13,  10.0000, 100.00,  10.0000, 2),
(4, 27,  20.0000,  85.00,  23.5294, 2),
(4,  4,   0.1000, 100.00,   0.1000, 1),
(5, 15,   1.0000, 100.00,   1.0000, 1),
(5, 16,   0.1000, 100.00,   0.1000, 1),
(5, 24,  10.0000, 100.00,  10.0000, 5),
(5, 22,   5.0000,  80.00,   6.2500, 1),
(5, 23,   1.0000, 100.00,   1.0000, 1),
(6, 26,   0.5000,  80.00,   0.6250, 1),
(6,  5, 500.0000, 100.00, 500.0000, 4),
(6,  7, 100.0000, 100.00, 100.0000, 4),
(6,  8,  20.0000, 100.00,  20.0000, 2),
(6,  9,  10.0000, 100.00,  10.0000, 2);
GO

-- ---------- Procedimientos ----------
INSERT INTO Procedimientos (IdReceta, NroPaso, Descripcion) VALUES
(1, 1, N'Lavar y secar las hojas de lechuga.'),
(1, 2, N'Cortar el pan en cubos y tostar.'),
(1, 3, N'Rallar el queso parmesano.'),
(1, 4, N'Salpimentar, cocinar y porcionar las supremas.'),
(1, 5, N'Salpimentar y asar los ajos envueltos en papel aluminio.'),
(1, 6, N'Mixear los ajos asados con la leche y el aceite.'),
(1, 7, N'Rectificar con sal y pimienta.'),
(2, 1, N'Lavar las papas y los puerros.'),
(2, 2, N'Cortar ambos vegetales en trozos medianos.'),
(2, 3, N'Saltear los puerros y la cebolla en aceite de oliva.'),
(2, 4, N'Agregar las papas y el caldo.'),
(2, 5, N'Hervir durante 1 hora y mixear.'),
(2, 6, N'Rectificar con sal y pimienta.'),
(3, 1, N'Derretir la manteca e incorporar la harina.'),
(3, 2, N'Agregar la leche de a poco sin dejar de revolver.'),
(3, 3, N'Cocinar a fuego bajo hasta obtener la consistencia deseada.'),
(3, 4, N'Salpimentar y agregar nuez moscada.'),
(4, 1, N'Limpiar el pollo, salpimentar y agregar aceite de oliva.'),
(4, 2, N'Hornear a 180 °C durante 40 minutos.'),
(4, 3, N'Lavar y cortar los vegetales en cubos.'),
(4, 4, N'Sofreír los vegetales en aceite de oliva.'),
(4, 5, N'Agregar y nacarar el arroz con los vegetales.'),
(4, 6, N'Agregar el caldo y salpimentar.'),
(5, 1, N'Mezclar harina, manteca y huevos hasta obtener una masa homogénea.'),
(5, 2, N'Estirar la masa, forrar el molde y refrigerar.'),
(5, 3, N'Pelar y cortar las manzanas en cubos.'),
(5, 4, N'Colocar las manzanas sobre la masa y espolvorear con azúcar.'),
(5, 5, N'Hornear a 180 °C durante 35 minutos hasta dorar.'),
(6, 1, N'Asar las zanahorias en papel aluminio.'),
(6, 2, N'Mixear las zanahorias con la leche y el aceite.'),
(6, 3, N'Rectificar con sal y pimienta.');
GO

-- ---------- Stock inicial auditado ----------
DECLARE @Admin NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE Email = 'admin@recetario.local');

INSERT INTO MovimientosStock (IdIngrediente, Tipo, Cantidad, IdUnidad, Fecha, UsuarioId, Observaciones)
SELECT IdIngrediente, 1, StockActual, IdUnidad, GETDATE(), @Admin, N'Stock inicial'
FROM Ingredientes
WHERE StockActual > 0 AND @Admin IS NOT NULL;
GO

PRINT 'Datos de demostración cargados.';
GO
