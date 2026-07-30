using RecetarioMVC.Models;

namespace RecetarioMVC.Helpers;

/// <summary>Nombre del depósito tal como se lo llama en la cocina.</summary>
public static class NombreDeposito
{
    public static string De(Deposito deposito) => deposito switch
    {
        Deposito.Camara => "Cámara",
        _ => "Bodega"
    };

    /// <summary>Qué guarda cada uno, para las ayudas de los formularios.</summary>
    public static string Descripcion(Deposito deposito) => deposito switch
    {
        Deposito.Camara => "Refrigerados: lácteos, carnes, frutas y verduras",
        _ => "Secos: harinas, aceites, especias, conservas"
    };
}
