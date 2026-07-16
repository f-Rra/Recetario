using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace RecetarioMVC.Services.Pdf;

/// <summary>Paleta Southex y bloques comunes de los PDFs (guía 01).</summary>
public static class EstiloPdf
{
    public const string Azul = "#1078B0";
    public const string AzulOscuro = "#0C5A85";
    public const string CelesteClaro = "#EAF3F9";
    public const string GrisTexto = "#2B3A45";
    public const string GrisSuave = "#6C7A85";

    public static void Encabezado(IContainer contenedor, string titulo, string subtitulo)
    {
        contenedor.Column(columna =>
        {
            columna.Item().Row(fila =>
            {
                fila.RelativeItem().Column(c =>
                {
                    c.Item().Text(titulo).FontSize(18).Bold().FontColor(Azul);
                    c.Item().Text(subtitulo).FontSize(10).FontColor(GrisSuave);
                });
                fila.ConstantItem(140).AlignRight().Column(c =>
                {
                    c.Item().AlignRight().Text("Recetario").FontSize(13).Bold().FontColor(Azul);
                    c.Item().AlignRight().Text("GRUPO SOUTHEX").FontSize(7).FontColor(GrisSuave).LetterSpacing(0.1f);
                });
            });
            columna.Item().PaddingTop(6).LineHorizontal(1.5f).LineColor(Azul);
        });
    }

    public static IContainer CeldaEncabezado(IContainer celda) =>
        celda.Background(CelesteClaro).PaddingVertical(4).PaddingHorizontal(6)
             .DefaultTextStyle(t => t.SemiBold().FontSize(9).FontColor(AzulOscuro));

    public static IContainer Celda(IContainer celda) =>
        celda.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3)
             .PaddingVertical(4).PaddingHorizontal(6)
             .DefaultTextStyle(t => t.FontSize(9.5f).FontColor(GrisTexto));

    public static void PieDePagina(IContainer contenedor)
    {
        contenedor.AlignCenter().Text(t =>
        {
            t.DefaultTextStyle(s => s.FontSize(8).FontColor(GrisSuave));
            t.Span("Generado el ");
            t.Span($"{DateTime.Now:dd/MM/yyyy HH:mm}");
            t.Span(" · Página ");
            t.CurrentPageNumber();
            t.Span(" de ");
            t.TotalPages();
        });
    }
}
