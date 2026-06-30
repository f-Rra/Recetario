CREATE OR ALTER VIEW vw_Comanda AS
SELECT
    r.IdReceta,
    r.Nombre       AS NombreReceta,
    c.Nombre       AS Clasificacion,
    i.Descripcion  AS NombreIngrediente,
    ir.CantNeta    AS Cantidad,
    u.Abreviatura  AS Unidad
FROM IngredientesxRecetas ir
    INNER JOIN Recetas        r ON r.IdReceta        = ir.IdReceta
    INNER JOIN Clasificaciones c ON c.IdClasificacion = r.IdClasificacion
    INNER JOIN Ingredientes    i ON i.IdIngrediente   = ir.IdIngrediente
    INNER JOIN Unidades        u ON u.IdUnidad        = ir.IdUnidad
WHERE r.Activo = 1;
