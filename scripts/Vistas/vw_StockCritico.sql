CREATE OR ALTER VIEW vw_StockCritico AS
SELECT
    i.Codigo,
    i.Descripcion,
    i.StockActual,
    i.StockMinimo,
    u.Abreviatura                 AS Unidad,
    i.StockMinimo - i.StockActual AS Diferencia
FROM Ingredientes i
    INNER JOIN Unidades u ON u.IdUnidad = i.IdUnidad
WHERE i.StockActual < i.StockMinimo;
