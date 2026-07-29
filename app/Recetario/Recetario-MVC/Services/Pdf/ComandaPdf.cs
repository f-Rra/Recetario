using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecetarioMVC.Helpers;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services.Pdf;

/// <summary>
/// Comanda de cocina: una sección por receta del pedido, con los ingredientes
/// ya escalados a los comensales y con las modificaciones aplicadas.
/// </summary>
public static class ComandaPdf
{
    public static byte[] Generar(ComandaPdfViewModel comanda)
    {
        return Document.Create(doc =>
        {
            doc.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);
                pagina.DefaultTextStyle(t => t.FontFamily("Segoe UI").FontSize(10).FontColor(EstiloPdf.GrisTexto));

                pagina.Header().Element(e => EstiloPdf.Encabezado(e,
                    "Comanda de cocina",
                    $"{comanda.Fecha:dd/MM/yyyy} · {comanda.Comensales} comensales" +
                    (string.IsNullOrEmpty(comanda.Usuario) ? "" : $" · Generada por {comanda.Usuario}")));

                pagina.Content().PaddingVertical(14).Column(columna =>
                {
                    columna.Spacing(18);

                    foreach (var seccion in comanda.Secciones)
                        columna.Item().Element(e => Seccion(e, seccion));
                });

                pagina.Footer().Element(EstiloPdf.PieDePagina);
            });
        }).GeneratePdf();
    }

    private static void Seccion(IContainer contenedor, SeccionComandaPdf seccion)
    {
        contenedor.Column(columna =>
        {
            columna.Spacing(8);

            columna.Item().Text(t =>
            {
                t.Span(seccion.Receta).FontSize(13).Bold().FontColor(EstiloPdf.AzulOscuro);
                if (!string.IsNullOrEmpty(seccion.Codigo))
                    t.Span($"  ({seccion.Codigo})").FontSize(9.5f).FontColor(EstiloPdf.GrisSuave);
            });

            columna.Item().Text($"Sector: {seccion.Sector}   ·   Responsable: {seccion.Responsable}")
                .FontSize(9.5f).FontColor(EstiloPdf.GrisSuave);

            // Ingredientes ya modificados: es lo que hay que cocinar
            columna.Item().Table(tabla =>
            {
                tabla.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(4);
                    c.RelativeColumn(2);
                });

                tabla.Header(h =>
                {
                    h.Cell().Element(EstiloPdf.CeldaEncabezado).Text("Ingrediente");
                    h.Cell().Element(EstiloPdf.CeldaEncabezado).AlignRight().Text("Cantidad");
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
                    .Background("#FCF3E9").Padding(8)
                    .BorderLeft(3).BorderColor("#D9822B")
                    .Column(cambios =>
                    {
                        cambios.Spacing(3);
                        cambios.Item().Text("Modificaciones aplicadas")
                            .FontSize(10).Bold().FontColor("#8A5216");

                        foreach (var modificacion in seccion.Modificaciones)
                            cambios.Item().Text($"•  {modificacion}").FontSize(9.5f).FontColor("#8A5216");
                    });
            }

            if (seccion.Pasos.Count > 0)
            {
                columna.Item().Text("Procedimiento").FontSize(11).Bold().FontColor(EstiloPdf.AzulOscuro);
                foreach (var paso in seccion.Pasos)
                {
                    columna.Item().PaddingLeft(6).Row(fila =>
                    {
                        fila.ConstantItem(18).Text($"{paso.NroPaso}.").SemiBold().FontColor(EstiloPdf.Azul);
                        fila.RelativeItem().Text(paso.Descripcion).FontSize(9.5f);
                    });
                }
            }
        });
    }
}
