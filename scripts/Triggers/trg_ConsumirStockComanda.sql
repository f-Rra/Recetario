CREATE OR ALTER TRIGGER trg_ConsumirStockComanda
ON Comandas
AFTER INSERT
AS
BEGIN
    INSERT INTO MovimientosStock (IdIngrediente, IdTipoMovimiento, Cantidad, IdUnidad, IdUsuario, Observaciones)
    SELECT ir.IdIngrediente,
           (SELECT IdTipoMovimiento FROM TiposMovimiento WHERE Nombre = 'salida'),
           CAST((ir.CantBruta / r.PorcionesBase) * c.Porciones AS DECIMAL(10,4)),
           ir.IdUnidad,
           c.IdUsuario,
           'Consumo comanda #' + CAST(c.IdComanda AS VARCHAR(10))
    FROM inserted c
        INNER JOIN Recetas              r  ON r.IdReceta  = c.IdReceta
        INNER JOIN IngredientesxRecetas ir ON ir.IdReceta = c.IdReceta;
END
GO
