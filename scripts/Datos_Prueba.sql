USE RecetarioDB;
GO

INSERT INTO Clasificaciones (Nombre) VALUES
('Entrada'),
('Plato Principal'),
('Postre'),
('Decoración'),
('Ensalada'),
('Salsa');
GO

INSERT INTO Unidades (Nombre, Abreviatura) VALUES
('Kilogramo', 'kg'),
('Gramo',     'g'),
('Litro',     'L'),
('Mililitro', 'ml'),
('Unidad',    'u');
GO

INSERT INTO TiposMovimiento (Nombre) VALUES
('entrada'),
('salida'),
('ajuste');
GO

INSERT INTO TiposModificacion (Nombre) VALUES
('sustitucion'),
('adicion'),
('eliminacion');
GO

INSERT INTO Personas (Nombre, Apellido, Email, Telefono, IdClasificacion) VALUES
('Carlos',  'Gómez',    NULL,                            NULL, 1),
('María',   'López',    NULL,                            NULL, 2),
('Pedro',   'Fernández',NULL,                            NULL, 3),
('Laura',   'Sánchez',  NULL,                            NULL, 5),
('Jorge',   'Torres',   NULL,                            NULL, 6),
('Facundo', 'Herrera',  'facundo.herrera@recetario.com', NULL, 2),
('Ana',     'Martínez', 'ana.martinez@recetario.com',    NULL, NULL);
GO

INSERT INTO Usuarios (IdPersona, Password, Rol) VALUES
(6, 'd000001', 'lider'),
(7, 'd000002', 'admin');
GO

INSERT INTO Proveedores (Nombre, Contacto, Telefono, Email, Direccion) VALUES
('Distribuidor Central', 'Roberto Díaz',   '011-4567-8901', 'ventas@distcentral.com',     'Av. Belgrano 1200, CABA'),
('Verduras del Sur',     'Graciela Pérez', '011-3456-7890', 'info@verdurasdelsur.com',    'Mercado Central, Puesto 45'),
('Lácteos Frescos',      'Miguel Torres',  '011-5678-9012', 'pedidos@lacteosfrescos.com', 'Ruta 8 km 32, GBA');
GO

INSERT INTO Ingredientes (Codigo, Descripcion, IdUnidad, StockActual, StockMinimo) VALUES
('ING001', 'Lechuga',            1,    2.5000,   2.0000),
('ING002', 'Crutones',           1,    0.5000,   1.0000),
('ING003', 'Supremas de pollo',  1,   15.0000,  10.0000),
('ING004', 'Queso parmesano',    1,    0.3000,   0.5000),
('ING005', 'Leche',              4, 1500.0000, 500.0000),
('ING006', 'Ajo',                5,    8.0000,   5.0000),
('ING007', 'Aceite de girasol',  4,  800.0000, 500.0000),
('ING008', 'Sal fina',           2,  500.0000, 100.0000),
('ING009', 'Pimienta negra',     2,   80.0000, 100.0000),
('ING010', 'Papas',              1,    8.0000,   5.0000),
('ING011', 'Puerros',            1,    1.5000,   2.0000),
('ING012', 'Cebolla',            1,    3.0000,   1.0000),
('ING013', 'Caldo de verduras',  2,  200.0000, 100.0000),
('ING014', 'Aceite de oliva',    4,  500.0000, 200.0000),
('ING015', 'Harina 0000',        1,    5.0000,   2.0000),
('ING016', 'Manteca',            1,    0.8000,   1.0000),
('ING017', 'Leche en polvo',     1,    3.0000,   2.0000),
('ING018', 'Nuez moscada',       2,   50.0000,  20.0000),
('ING019', 'Morrón',             1,    1.5000,   1.0000),
('ING020', 'Apio',               1,    2.0000,   1.0000),
('ING021', 'Arroz largo fino',   1,    4.0000,   2.0000),
('ING022', 'Manzanas',           1,    8.0000,   5.0000),
('ING023', 'Azúcar',             1,    3.0000,   1.0000),
('ING024', 'Huevos',             5,   30.0000,  12.0000),
('ING025', 'Atún en lata',       2,  300.0000, 200.0000),
('ING026', 'Zanahorias',         1,    3.0000,   1.0000),
('ING027', 'Perejil',            2,   40.0000,  50.0000);
GO

INSERT INTO Recetas (Codigo, Nombre, IdClasificacion, PorcionesBase, Activo, Imagen) VALUES
('REC001', 'Ensalada César',           5,  10, 1, NULL),
('REC002', 'Sopa de papas y puerros',  1,   1, 1, NULL),
('REC003', 'Salsa blanca',             6,  10, 1, NULL),
('REC004', 'Pollo al horno con arroz', 2,  10, 1, NULL),
('REC005', 'Tarta de manzana',         3, 100, 1, NULL),
('REC006', 'Mayonesa de zanahorias',   4,   1, 1, NULL);
GO

INSERT INTO PrecioxIngrediente (IdIngrediente, IdProveedor, Precio, FechaVigencia) VALUES
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

INSERT INTO IngredientesxRecetas (IdReceta, IdIngrediente, CantNeta, Rendimiento, CantBruta, IdUnidad) VALUES
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

INSERT INTO Procedimientos (IdReceta, NroPaso, Descripcion) VALUES
(1, 1, 'Lavar y secar las hojas de lechuga.'),
(1, 2, 'Cortar el pan en cubos y tostar.'),
(1, 3, 'Rallar el queso parmesano.'),
(1, 4, 'Salpimentar, cocinar y porcionar las supremas.'),
(1, 5, 'Salpimentar y asar los ajos envueltos en papel aluminio.'),
(1, 6, 'Mixear los ajos asados con la leche y el aceite.'),
(1, 7, 'Rectificar con sal y pimienta.'),
(2, 1, 'Lavar las papas y los puerros.'),
(2, 2, 'Cortar ambos vegetales en trozos medianos.'),
(2, 3, 'Saltear los puerros y la cebolla en aceite de oliva.'),
(2, 4, 'Agregar las papas y el caldo.'),
(2, 5, 'Hervir durante 1 hora y mixear.'),
(2, 6, 'Rectificar con sal y pimienta.'),
(3, 1, 'Derretir la manteca e incorporar la harina.'),
(3, 2, 'Agregar la leche de a poco sin dejar de revolver.'),
(3, 3, 'Cocinar a fuego bajo hasta obtener la consistencia deseada.'),
(3, 4, 'Salpimentar y agregar nuez moscada.'),
(4, 1, 'Limpiar el pollo, salpimentar y agregar aceite de oliva.'),
(4, 2, 'Hornear a 180 °C durante 40 minutos.'),
(4, 3, 'Lavar y cortar los vegetales en cubos.'),
(4, 4, 'Sofreír los vegetales en aceite de oliva.'),
(4, 5, 'Agregar y nacarar el arroz con los vegetales.'),
(4, 6, 'Agregar el caldo y salpimentar.'),
(5, 1, 'Mezclar harina, manteca y huevos hasta obtener una masa homogénea.'),
(5, 2, 'Estirar la masa, forrar el molde y refrigerar.'),
(5, 3, 'Pelar y cortar las manzanas en cubos.'),
(5, 4, 'Colocar las manzanas sobre la masa y espolvorear con azúcar.'),
(5, 5, 'Hornear a 180 °C durante 35 minutos hasta dorar.'),
(6, 1, 'Asar las zanahorias en papel aluminio.'),
(6, 2, 'Mixear las zanahorias con la leche y el aceite.'),
(6, 3, 'Rectificar con sal y pimienta.');
GO

INSERT INTO Comandas (IdReceta, Fecha, Porciones, IdUsuario, IdPersona) VALUES
(1, '2026-05-20', 10, 1, 4),
(4, '2026-05-20', 10, 1, 2),
(2, '2026-05-21',  5, 2, 1);
GO

INSERT INTO Modificaciones (IdComanda, IdTipoModificacion, IdIngredienteOriginal, IdIngredienteReemplazo, Cantidad, IdUnidad) VALUES
(1, 3, 1, NULL, 0.3200, 1),
(1, 1, 3,   25, 0.0500, 1);
GO

INSERT INTO Costos (IdReceta, Fecha, Porciones, CostoTotal, CostoUnitario, IdUsuario) VALUES
(1, '2026-05-20', 10,  1925.6600,  192.5660, 2),
(4, '2026-05-19', 10, 10991.8600, 1099.1860, 2);
GO

INSERT INTO MovimientosStock (IdIngrediente, IdTipoMovimiento, Cantidad, IdUnidad, Fecha, IdUsuario, Observaciones) VALUES
( 1, 1,  5.0000, 1, '2026-05-19 08:00:00', 2, 'Compra semanal'),
( 3, 1, 20.0000, 1, '2026-05-19 08:30:00', 2, 'Compra semanal'),
(10, 1, 15.0000, 1, '2026-05-19 09:00:00', 2, 'Compra semanal'),
(11, 1,  5.0000, 1, '2026-05-19 09:30:00', 2, 'Compra semanal'),
(21, 1, 10.0000, 1, '2026-05-19 10:00:00', 2, 'Compra semanal'),
( 3, 2,  2.0000, 1, '2026-05-20 12:00:00', 1, 'Elaboración comanda 2'),
( 1, 2,  0.4000, 1, '2026-05-20 11:30:00', 1, 'Elaboración comanda 1'),
(12, 3,  3.0000, 1, '2026-05-20 07:00:00', 2, 'Corrección de inventario');
GO
