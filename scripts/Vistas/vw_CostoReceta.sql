CREATE OR ALTER VIEW vw_CostoReceta AS
WITH PrecioActual AS (
    SELECT
        IdIngrediente,
        Precio,
        ROW_NUMBER() OVER (PARTITION BY IdIngrediente ORDER BY FechaVigencia DESC, IdProveedor ASC) AS rn
    FROM PrecioxIngrediente
)
SELECT
    r.IdReceta,
    r.Nombre                 AS NombreReceta,
    i.Descripcion            AS NombreIngrediente,
    ir.CantBruta             AS CantBruta,
    u.Abreviatura            AS Unidad,
    pv.Precio                AS CostoUnitario,
    ir.CantBruta * pv.Precio AS CostoIngrediente,
    (
        SELECT SUM(ir2.CantBruta * pv2.Precio)
        FROM IngredientesxRecetas ir2
            INNER JOIN PrecioActual pv2 ON pv2.IdIngrediente = ir2.IdIngrediente AND pv2.rn = 1
        WHERE ir2.IdReceta = ir.IdReceta
    )                        AS CostoTotalReceta
FROM IngredientesxRecetas ir
    INNER JOIN Recetas      r  ON r.IdReceta      = ir.IdReceta
    INNER JOIN Ingredientes i  ON i.IdIngrediente = ir.IdIngrediente
    INNER JOIN Unidades     u  ON u.IdUnidad      = ir.IdUnidad
    INNER JOIN PrecioActual pv ON pv.IdIngrediente = ir.IdIngrediente AND pv.rn = 1
WHERE r.Activo = 1;
GO
