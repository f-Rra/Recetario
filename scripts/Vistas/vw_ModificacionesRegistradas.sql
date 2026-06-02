CREATE OR ALTER VIEW vw_ModificacionesRegistradas AS
SELECT
    c.Fecha,
    r.Nombre           AS NombreReceta,
    tm.Nombre          AS Tipo,
    iOrig.Descripcion  AS IngredienteOriginal,
    iRemp.Descripcion  AS IngredienteReemplazo,
    m.Cantidad,
    u.Abreviatura      AS Unidad
FROM Modificaciones m
    INNER JOIN Comandas          c     ON c.IdComanda           = m.IdComanda
    INNER JOIN Recetas           r     ON r.IdReceta            = c.IdReceta
    INNER JOIN TiposModificacion tm    ON tm.IdTipoModificacion = m.IdTipoModificacion
    LEFT  JOIN Ingredientes      iOrig ON iOrig.IdIngrediente   = m.IdIngredienteOriginal
    LEFT  JOIN Ingredientes      iRemp ON iRemp.IdIngrediente   = m.IdIngredienteReemplazo
    INNER JOIN Unidades          u     ON u.IdUnidad            = m.IdUnidad;
