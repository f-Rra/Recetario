CREATE OR ALTER PROCEDURE sp_AjustarReceta
    @IdReceta   INT,
    @Comensales INT
AS
BEGIN
    DECLARE @PorcionesBase INT;

    SELECT @PorcionesBase = PorcionesBase
    FROM Recetas
    WHERE IdReceta = @IdReceta;

    IF @PorcionesBase IS NULL
    BEGIN
        RAISERROR('No existe una receta con el ID indicado.', 16, 1);
        RETURN;
    END

    -- Devuelve las cantidades ajustadas al numero de comensales pedido.
    -- Formula: (CantNeta / PorcionesBase) * @Comensales
    SELECT
        r.Nombre                                                              AS NombreReceta,
        i.Descripcion                                                         AS NombreIngrediente,
        CAST((ir.CantNeta / r.PorcionesBase) * @Comensales AS DECIMAL(10,4)) AS CantidadAjustada,
        u.Abreviatura                                                         AS Unidad
    FROM IngredientesxRecetas ir
        INNER JOIN Recetas      r ON r.IdReceta      = ir.IdReceta
        INNER JOIN Ingredientes i ON i.IdIngrediente = ir.IdIngrediente
        INNER JOIN Unidades     u ON u.IdUnidad      = ir.IdUnidad
    WHERE ir.IdReceta = @IdReceta;
END
GO
