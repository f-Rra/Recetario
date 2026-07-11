namespace RecetarioMVC.Models;

/// <summary>
/// Reemplaza a la tabla TiposMovimiento del schema viejo: los valores son fijos
/// porque la lógica de stock depende de ellos (entrada suma, salida resta).
/// </summary>
public enum TipoMovimiento
{
    Entrada = 1,
    Salida = 2,
    Ajuste = 3
}

/// <summary>
/// Reemplaza a la tabla TiposModificacion del schema viejo.
/// </summary>
public enum TipoModificacion
{
    Sustitucion = 1,
    Adicion = 2,
    Eliminacion = 3
}
