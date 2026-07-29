using System.Globalization;
using RecetarioMVC.Helpers;

namespace RecetarioMVC.Tests;

public class FormatoCantidadTests
{
    public FormatoCantidadTests()
    {
        // La app corre en es-AR: coma decimal y punto de miles
        CultureInfo.CurrentCulture = new CultureInfo("es-AR");
    }

    [Theory]
    [InlineData(15, "kg", "15 kg")]        // sin decimales de relleno
    [InlineData(22, "u", "22 u")]
    [InlineData(294, "g", "294 g")]
    [InlineData(7.53, "kg", "7,53 kg")]    // los decimales que sí aportan quedan
    [InlineData(3.56, "u", "3,56 u")]
    public void MuestraElNumeroSinCerosDeRelleno(decimal cantidad, string unidad, string esperado)
    {
        Assert.Equal(esperado, FormatoCantidad.Formatear(cantidad, unidad));
    }

    [Theory]
    [InlineData(1800, "g", "1,8 kg")]
    [InlineData(20736, "g", "20,74 kg")]
    [InlineData(1000, "g", "1 kg")]
    [InlineData(999, "g", "999 g")]        // debajo de mil se queda en gramos
    public void MuchosGramosPasanAKilos(decimal cantidad, string unidad, string esperado)
    {
        Assert.Equal(esperado, FormatoCantidad.Formatear(cantidad, unidad));
    }

    [Theory]
    [InlineData(6000, "ml", "6 L")]
    [InlineData(10800, "ml", "10,8 L")]
    [InlineData(800, "ml", "800 ml")]      // debajo de mil se queda en mililitros
    public void MuchosMililitrosPasanALitros(decimal cantidad, string unidad, string esperado)
    {
        Assert.Equal(esperado, FormatoCantidad.Formatear(cantidad, unidad));
    }

    [Theory]
    [InlineData(0.8, "kg", "800 g")]
    [InlineData(0.625, "kg", "625 g")]
    [InlineData(0.04, "kg", "40 g")]
    [InlineData(0.5, "L", "500 ml")]
    public void MenosDeLaUnidadGrandeBajaALaChica(decimal cantidad, string unidad, string esperado)
    {
        Assert.Equal(esperado, FormatoCantidad.Formatear(cantidad, unidad));
    }

    [Theory]
    [InlineData(0, "kg", "0 kg")]          // el cero no se convierte
    [InlineData(0, "g", "0 g")]
    [InlineData(1, "kg", "1 kg")]          // el límite exacto no baja
    [InlineData(5, "u", "5 u")]            // las unidades nunca se convierten
    [InlineData(1500, "u", "1.500 u")]
    public void CasosBorde(decimal cantidad, string unidad, string esperado)
    {
        Assert.Equal(esperado, FormatoCantidad.Formatear(cantidad, unidad));
    }

    [Fact]
    public void LaConversionNoPierdeLaCantidadReal()
    {
        // 23,5294 g de perejil: se redondea para mostrar, no para calcular
        Assert.Equal("23,53 g", FormatoCantidad.Formatear(23.5294m, "g"));
        Assert.Equal("1,23 kg", FormatoCantidad.Formatear(1234.5m, "g"));
    }
}
