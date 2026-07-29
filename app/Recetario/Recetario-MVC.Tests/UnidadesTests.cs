using RecetarioMVC.Helpers;

namespace RecetarioMVC.Tests;

public class UnidadesTests
{
    [Theory]
    [InlineData("kg", "kg")]
    [InlineData("kg", "g")]
    [InlineData("g", "kg")]
    [InlineData("L", "ml")]
    [InlineData("ml", "L")]
    [InlineData("u", "u")]
    public void SonEquivalentesLasDeLaMismaFamilia(string a, string b)
    {
        Assert.True(Unidades.SonEquivalentes(a, b));
    }

    [Theory]
    [InlineData("kg", "L")]
    [InlineData("g", "ml")]
    [InlineData("kg", "u")]
    [InlineData("u", "g")]
    public void NoSonEquivalentesLasDeFamiliasDistintas(string a, string b)
    {
        Assert.False(Unidades.SonEquivalentes(a, b));
    }

    [Theory]
    [InlineData(2, "kg", "g", 2000)]
    [InlineData(1500, "g", "kg", 1.5)]
    [InlineData(0.625, "kg", "g", 625)]
    [InlineData(3, "L", "ml", 3000)]
    [InlineData(250, "ml", "L", 0.25)]
    [InlineData(5, "kg", "kg", 5)]
    public void ConvierteEntreUnidadesEquivalentes(
        decimal cantidad, string desde, string hacia, decimal esperado)
    {
        Assert.Equal(esperado, Unidades.Convertir(cantidad, desde, hacia));
    }

    [Theory]
    [InlineData("kg", "L")]
    [InlineData("g", "u")]
    [InlineData("ml", "kg")]
    public void NoConvierteEntreFamiliasDistintas(string desde, string hacia)
    {
        Assert.Null(Unidades.Convertir(1, desde, hacia));
    }

    [Fact]
    public void IdaYVueltaDevuelveLaMismaCantidad()
    {
        var enGramos = Unidades.Convertir(1.25m, "kg", "g");
        Assert.Equal(1250m, enGramos);
        Assert.Equal(1.25m, Unidades.Convertir(enGramos!.Value, "g", "kg"));
    }
}
