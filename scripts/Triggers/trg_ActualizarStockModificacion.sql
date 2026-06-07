CREATE OR ALTER TRIGGER trg_ActualizarStockModificacion
ON Modificaciones
AFTER INSERT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @IdIngredienteOriginal  INT;
        DECLARE @IdIngredienteReemplazo INT;
        DECLARE @Cantidad               DECIMAL(10,4);

        SELECT @IdIngredienteOriginal  = i.IdIngredienteOriginal,
               @IdIngredienteReemplazo = i.IdIngredienteReemplazo,
               @Cantidad               = i.Cantidad
        FROM inserted i;

        IF @IdIngredienteOriginal IS NOT NULL
            UPDATE Ingredientes
            SET StockActual = StockActual + @Cantidad
            WHERE IdIngrediente = @IdIngredienteOriginal;

        IF @IdIngredienteReemplazo IS NOT NULL
            UPDATE Ingredientes
            SET StockActual = StockActual - @Cantidad
            WHERE IdIngrediente = @IdIngredienteReemplazo;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT ERROR_MESSAGE();
    END CATCH
END
GO
