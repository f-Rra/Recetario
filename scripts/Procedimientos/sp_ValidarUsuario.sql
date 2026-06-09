CREATE OR ALTER PROCEDURE sp_ValidarUsuario
    @Email    VARCHAR(150),
    @Password VARCHAR(255)
AS
BEGIN
    SELECT
        u.IdUsuario,
        u.IdPersona,
        u.Rol,
        p.Nombre,
        p.Apellido,
        p.Email,
        p.IdClasificacion,
        c.Nombre AS NombreClasificacion
    FROM Usuarios u
    INNER JOIN Personas p       ON p.IdPersona = u.IdPersona
    LEFT  JOIN Clasificaciones c ON c.IdClasificacion = p.IdClasificacion
    WHERE p.Email   = @Email
      AND u.Password = @Password;
END
GO
