CREATE OR ALTER TRIGGER trg_ActualizarStockMovimiento
ON MovimientosStock
AFTER INSERT
AS
BEGIN
    UPDATE i
    SET i.StockActual = i.StockActual + ins.Cantidad
    FROM Ingredientes i
        INNER JOIN inserted        ins ON ins.IdIngrediente    = i.IdIngrediente
        INNER JOIN TiposMovimiento tm  ON tm.IdTipoMovimiento  = ins.IdTipoMovimiento
    WHERE tm.Nombre = 'entrada';

    UPDATE i
    SET i.StockActual = i.StockActual - ins.Cantidad
    FROM Ingredientes i
        INNER JOIN inserted        ins ON ins.IdIngrediente    = i.IdIngrediente
        INNER JOIN TiposMovimiento tm  ON tm.IdTipoMovimiento  = ins.IdTipoMovimiento
    WHERE tm.Nombre = 'salida';

    UPDATE i
    SET i.StockActual = ins.Cantidad
    FROM Ingredientes i
        INNER JOIN inserted        ins ON ins.IdIngrediente    = i.IdIngrediente
        INNER JOIN TiposMovimiento tm  ON tm.IdTipoMovimiento  = ins.IdTipoMovimiento
    WHERE tm.Nombre = 'ajuste';
END
GO
