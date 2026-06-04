CREATE OR ALTER PROCEDURE sp_RegistrarComanda
    @IdReceta  INT,
    @Porciones INT,
    @IdUsuario INT
AS
BEGIN
    DECLARE @IdClasificacion INT;
    DECLARE @IdPersona       INT;
    DECLARE @IdComanda       INT;

    SELECT @IdClasificacion = IdClasificacion
    FROM Recetas
    WHERE IdReceta = @IdReceta;

    IF @IdClasificacion IS NULL
    BEGIN
        RAISERROR('No existe una receta con el ID indicado', 16, 1);
        RETURN;
    END

    SELECT TOP 1 @IdPersona = p.IdPersona
    FROM Personas p
    WHERE p.IdClasificacion = @IdClasificacion
      AND NOT EXISTS (
          SELECT 1 FROM Usuarios u WHERE u.IdPersona = p.IdPersona
      );

    IF @IdPersona IS NULL
    BEGIN
        RAISERROR('No hay integrante de cocina disponible para la clasificacion de esta receta', 16, 1);
        RETURN;
    END

    INSERT INTO Comandas (IdReceta, Fecha, Porciones, IdUsuario, IdPersona)
    VALUES (@IdReceta, CAST(GETDATE() AS DATE), @Porciones, @IdUsuario, @IdPersona);

    SET @IdComanda = SCOPE_IDENTITY();

    SELECT @IdComanda AS IdComanda;
END
GO
