using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecetarioMVC.Helpers;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services.Pdf;

/// <summary>
/// Comanda de cocina: una página por receta, titulada con su nombre, con las
/// cantidades ya escaladas a los comensales y las modificaciones aplicadas.
/// Así cada responsable de sector se queda con su hoja.
/// </summary>
public static class ComandaPdf
{
    public static byte[] Generar(ComandaPdfViewModel comanda)
    {
        return Document.Create(doc =>
        {
            foreach (var seccion in comanda.Secciones)
            {
                doc.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(40);
                    pagina.DefaultTextStyle(t => t.FontFamily("Segoe UI").FontSize(12).FontColor(EstiloPdf.GrisTexto));

                    pagina.Header().Element(e => EstiloPdf.Encabezado(e,
                        seccion.Receta,
                        $"{comanda.Fecha:dd/MM/yyyy} · {comanda.Comensales} comensales"));

                    pagina.Content().PaddingVertical(16).Element(e => Contenido(e, seccion));
                });
            }
        }).GeneratePdf();
    }

    private static void Contenido(IContainer contenedor, SeccionComandaPdf seccion)
    {
        contenedor.Column(columna =>
        {
            columna.Spacing(14);

            columna.Item().Text(t =>
            {
                t.DefaultTextStyle(s => s.FontSize(12).FontColor(EstiloPdf.GrisSuave));
                t.Span("Sector: ");
                t.Span(seccion.Sector).SemiBold().FontColor(EstiloPdf.GrisTexto);
                t.Span("     Responsable: ");
                t.Span(seccion.Responsable).SemiBold().FontColor(EstiloPdf.GrisTexto);
            });

            // Lo que hay que cocinar, con las modificaciones ya aplicadas
            columna.Item().Table(tabla =>
            {
                tabla.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(4);
                    c.RelativeColumn(2);
                });

                tabla.Header(h =>
                {
                    h.Cell().Element(EstiloPdf.CeldaEncabezado).Text("Ingredientes");
                    h.Cell().Element(EstiloPdf.CeldaEncabezado).AlignRight().Text("Cantidades");
                });

                foreach (var ing in seccion.Ingredientes)
                {
                    tabla.Cell().Element(EstiloPdf.Celda).Text(ing.Ingrediente);
                    tabla.Cell().Element(EstiloPdf.Celda).AlignRight()
                        .Text(FormatoCantidad.Formatear(ing.Cantidad, ing.Unidad));
                }
            });

            // Qué se cambió respecto de la receta original
            if (seccion.Modificaciones.Count > 0)
            {
                columna.Item()
                    .Background("#FCF3E9")
                    .BorderLeft(4).BorderColor("#D9822B")
                    .PaddingVertical(10).PaddingLeft(16).PaddingRight(12)
                    .Column(cambios =>
                    {
                        cambios.Spacing(5);
                        cambios.Item().Text("Cambios en la receta")
                            .FontSize(13).Bold().FontColor("#8A5216");

                        foreach (var modificacion in seccion.Modificaciones)
                            cambios.Item().Text($"•  {modificacion}").FontSize(12).FontColor("#8A5216");
                    });
            }

            if (seccion.Pasos.Count > 0)
            {
                columna.Item().Text("Procedimiento").FontSize(14).Bold().FontColor(EstiloPdf.AzulOscuro);
                foreach (var paso in seccion.Pasos)
                {
                    columna.Item().PaddingLeft(6).Row(fila =>
                    {
                        fila.ConstantItem(22).Text($"{paso.NroPaso}.").SemiBold().FontColor(EstiloPdf.Azul);
                        fila.RelativeItem().Text(paso.Descripcion).FontSize(12);
                    });
                }
            }
        });
    }
}
