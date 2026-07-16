using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services.Pdf;

/// <summary>Informe de costeo registrado (reemplaza a GeneradorPDF.GenerarCosto).</summary>
public static class CosteoPdf
{
    public static byte[] Generar(CosteoRegistradoViewModel costeo)
    {
        return Document.Create(doc =>
        {
            doc.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);
                pagina.DefaultTextStyle(t => t.FontFamily("Segoe UI").FontSize(10).FontColor(EstiloPdf.GrisTexto));

                pagina.Header().Element(e => EstiloPdf.Encabezado(e,
                    "Informe de costeo",
                    $"{costeo.Fecha:dd/MM/yyyy} · Registrado por {costeo.Usuario}"));

                pagina.Content().PaddingVertical(14).Column(columna =>
                {
                    columna.Spacing(12);

                    columna.Item().Text(t =>
                    {
                        t.Span(costeo.Receta).FontSize(14).Bold();
                        t.Span($"  ({costeo.Codigo})").FontSize(10).FontColor(EstiloPdf.GrisSuave);
                    });
                    columna.Item().Text($"Costeo para {costeo.Porciones} porciones")
                        .FontSize(10).FontColor(EstiloPdf.GrisSuave);

                    columna.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(4);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                            c.RelativeColumn(2);
                        });

                        tabla.Header(h =>
                        {
                            h.Cell().Element(EstiloPdf.CeldaEncabezado).Text("Ingrediente");
                            h.Cell().Element(EstiloPdf.CeldaEncabezado).AlignRight().Text("Cant. bruta");
                            h.Cell().Element(EstiloPdf.CeldaEncabezado).AlignRight().Text("Precio");
                            h.Cell().Element(EstiloPdf.CeldaEncabezado).AlignRight().Text("Subtotal");
                        });

                        foreach (var d in costeo.Detalles)
                        {
                            tabla.Cell().Element(EstiloPdf.Celda).Text(d.Ingrediente);
                            tabla.Cell().Element(EstiloPdf.Celda).AlignRight().Text($"{d.CantBruta:N2} {d.Unidad}");
                            tabla.Cell().Element(EstiloPdf.Celda).AlignRight().Text($"$ {d.PrecioUnitario:N2}");
                            tabla.Cell().Element(EstiloPdf.Celda).AlignRight().Text($"$ {d.Subtotal:N2}");
                        }
                    });

                    columna.Item().AlignRight().Column(totales =>
                    {
                        totales.Spacing(2);
                        totales.Item().Text($"Costo total:  $ {costeo.CostoTotal:N2}")
                            .FontSize(12).Bold().FontColor(EstiloPdf.AzulOscuro);
                        totales.Item().Text($"Costo por porción:  $ {costeo.CostoUnitario:N2}")
                            .FontSize(11).SemiBold().FontColor(EstiloPdf.Azul);
                    });
                });

                pagina.Footer().Element(EstiloPdf.PieDePagina);
            });
        }).GeneratePdf();
    }
}
