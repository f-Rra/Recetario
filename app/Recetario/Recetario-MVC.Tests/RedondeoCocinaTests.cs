using RecetarioMVC.Helpers;

namespace RecetarioMVC.Tests;

public class RedondeoCocinaTests
{
    [Theory]
    // Unidades contables: el cocinero saca piezas enteras
    [InlineData(1.51, "u", 2)]
    [InlineData(1.2, "u", 1)]
    [InlineData(1.5, "u", 2)]   // el empate va para arriba
    [InlineData(12, "u", 12)]
    // Gramos y mililitros: la fracción no se pesa
    [InlineData(6.8, "g", 7)]
    [InlineData(3.4, "g", 3)]
    [InlineData(340, "ml", 340)]
    public void RedondeaAlEnteroLoQueSeCuentaOSePesa(decimal cantidad, string unidad, decimal esperado)
    {
        Assert.Equal(esperado, RedondeoCocina.Redondear(cantidad, unidad));
    }

    [Theory]
    // Kilos y litros: al medio, que es como se habla en la cocina
    [InlineData(1.89, "kg", 2)]
    [InlineData(3.2, "kg", 3)]
    [InlineData(1.4, "kg", 1.5)]
    [InlineData(1.2, "kg", 1)]
    [InlineData(12.4, "kg", 12.5)]
    [InlineData(1.25, "L", 1.5)] // el empate va para arriba
    [InlineData(2, "L", 2)]
    public void RedondeaAlMedioLosKilosYLitros(decimal cantidad, string unidad, decimal esperado)
    {
        Assert.Equal(esperado, RedondeoCocina.Redondear(cantidad, unidad));
    }

    [Fact]
    public void AplicaLaReglaDeLaUnidadEnQueSeLee_NoLaGuardada()
    {
        // 3200 g se leen como 3,2 kg → 3 kg, y vuelven a gramos para descontar
        Assert.Equal(3000m, RedondeoCocina.Redondear(3200m, "g"));

        // 800 g se leen en gramos aunque el ingrediente esté en kilos
        Assert.Equal(0.8m, RedondeoCocina.Redondear(0.8m, "kg"));

        // 1600 ml → 1,6 L → 1,5 L → 1500 ml
        Assert.Equal(1500m, RedondeoCocina.Redondear(1600m, "ml"));
    }

    [Fact]
    public void UnaCantidadChicaNuncaDesapareceDeLaReceta()
    {
        // Redondear al más cercano dejaría estos en cero: quedan en el mínimo.
        // Los kilos no llegan a este caso: abajo de 1 kg ya se leen en gramos
        Assert.Equal(1m, RedondeoCocina.Redondear(0.3m, "u"));
        Assert.Equal(1m, RedondeoCocina.Redondear(0.04m, "g"));
        // 0,2 g se leen en gramos, quedan en 1 g y vuelven a kilos para descontar
        Assert.Equal(0.001m, RedondeoCocina.Redondear(0.0002m, "kg"));
    }

    [Fact]
    public void CeroYNegativosQuedanComoEstan()
    {
        Assert.Equal(0m, RedondeoCocina.Redondear(0m, "kg"));
        Assert.Equal(-2.4m, RedondeoCocina.Redondear(-2.4m, "kg"));
    }
}
