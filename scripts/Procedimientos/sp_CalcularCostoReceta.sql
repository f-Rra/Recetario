CREATE OR ALTER PROCEDURE sp_CalcularCostoReceta
    @IdReceta  INT,
    @Porciones INT,
    @IdUsuario INT
AS
BEGIN
    DECLARE @CostoTotal    DECIMAL(12,4);
    DECLARE @CostoUnitario DECIMAL(12,4);
    DECLARE @IdCosto       INT;

    IF NOT EXISTS (SELECT 1 FROM Recetas WHERE IdReceta = @IdReceta)
    BEGIN
        RAISERROR('No existe una receta con el ID indicado.', 16, 1);
        RETURN;
    END

    SELECT @CostoTotal = SUM(ir.CantBruta * pv.Precio)
    FROM IngredientesxRecetas ir
        INNER JOIN PrecioxIngrediente pv ON pv.IdIngrediente = ir.IdIngrediente
    WHERE ir.IdReceta = @IdReceta
      AND pv.FechaVigencia = (
          SELECT MAX(pv2.FechaVigencia)
          FROM PrecioxIngrediente pv2
          WHERE pv2.IdIngrediente = pv.IdIngrediente
      );

    IF @CostoTotal IS NULL
    BEGIN
        RAISERROR('No se encontraron precios vigentes para los ingredientes de esta receta.', 16, 1);
        RETURN;
    END

    SET @CostoUnitario = @CostoTotal / @Porciones;

    INSERT INTO Costos (IdReceta, Fecha, Porciones, CostoTotal, CostoUnitario, IdUsuario)
    VALUES (@IdReceta, CAST(GETDATE() AS DATE), @Porciones, @CostoTotal, @CostoUnitario, @IdUsuario);

    SET @IdCosto = SCOPE_IDENTITY();

    SELECT @IdCosto       AS IdCosto,
           @CostoTotal    AS CostoTotal,
           @CostoUnitario AS CostoUnitario;
END
GO
