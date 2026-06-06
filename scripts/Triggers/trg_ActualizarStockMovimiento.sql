CREATE OR ALTER TRIGGER trg_ActualizarStockMovimiento
ON MovimientosStock
AFTER INSERT
AS
BEGIN
    DECLARE @IdIngrediente    INT;
    DECLARE @IdTipoMovimiento INT;
    DECLARE @Cantidad         DECIMAL(10,4);
    DECLARE @TipoNombre       VARCHAR(30);

    SELECT @IdIngrediente    = i.IdIngrediente,
           @IdTipoMovimiento = i.IdTipoMovimiento,
           @Cantidad         = i.Cantidad
    FROM inserted i;

    SELECT @TipoNombre = Nombre
    FROM TiposMovimiento
    WHERE IdTipoMovimiento = @IdTipoMovimiento;

    IF @TipoNombre = 'entrada'
        UPDATE Ingredientes
        SET StockActual = StockActual + @Cantidad
        WHERE IdIngrediente = @IdIngrediente;

    ELSE IF @TipoNombre = 'salida'
        UPDATE Ingredientes
        SET StockActual = StockActual - @Cantidad
        WHERE IdIngrediente = @IdIngrediente;

    ELSE IF @TipoNombre = 'ajuste'
        UPDATE Ingredientes
        SET StockActual = @Cantidad
        WHERE IdIngrediente = @IdIngrediente;
END
GO
