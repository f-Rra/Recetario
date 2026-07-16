using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services.Pdf;

/// <summary>Comanda de cocina imprimible (reemplaza a GeneradorPDF.GenerarComanda).</summary>
public static class ComandaPdf
{
    public static byte[] Generar(ComandaDetalleViewModel comanda)
    {
        return Document.Create(doc =>
        {
            doc.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);
                pagina.DefaultTextStyle(t => t.FontFamily("Segoe UI").FontSize(10).FontColor(EstiloPdf.GrisTexto));

                pagina.Header().Element(e => EstiloPdf.Encabezado(e,
                    $"Comanda #{comanda.IdComanda}",
                    $"{comanda.Fecha:dd/MM/yyyy} · Responsable: {comanda.Responsable} · Registrada por {comanda.Usuario}"));

                pagina.Content().PaddingVertical(14).Column(columna =>
                {
                    columna.Spacing(12);

                    columna.Item().Text(t =>
                    {
                        t.Span(comanda.Receta).FontSize(14).Bold();
                        t.Span($"  ({comanda.Codigo})").FontSize(10).FontColor(EstiloPdf.GrisSuave);
                    });
                    columna.Item().Text($"Sector: {comanda.Clasificacion}   ·   Porciones: {comanda.Porciones}")
                        .FontSize(10).FontColor(EstiloPdf.GrisSuave);

                    // Ingredientes escalados
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

                        foreach (var ing in comanda.Ingredientes)
                        {
                            tabla.Cell().Element(EstiloPdf.Celda).Text(ing.Ingrediente);
                            tabla.Cell().Element(EstiloPdf.Celda).AlignRight()
                                .Text($"{ing.Cantidad:N2} {ing.Unidad}");
                        }
                    });

                    // Modificaciones
                    if (comanda.Modificaciones.Count > 0)
                    {
                        columna.Item().Text("Modificaciones").FontSize(11).Bold().FontColor(EstiloPdf.AzulOscuro);
                        foreach (var m in comanda.Modificaciones)
                        {
                            var descripcion = m.Tipo switch
                            {
                                Models.TipoModificacion.Sustitucion =>
                                    $"Sustituir {m.IngredienteOriginal} por {m.IngredienteReemplazo} ({m.Cantidad:N2} {m.Unidad})",
                                Models.TipoModificacion.Adicion =>
                                    $"Agregar {m.IngredienteReemplazo} ({m.Cantidad:N2} {m.Unidad})",
                                _ =>
                                    $"Quitar {m.IngredienteOriginal} ({m.Cantidad:N2} {m.Unidad})"
                            };
                            columna.Item().PaddingLeft(10).Text($"•  {descripcion}").FontSize(9.5f);
                        }
                    }

                    // Procedimiento
                    if (comanda.Pasos.Count > 0)
                    {
                        columna.Item().Text("Procedimiento").FontSize(11).Bold().FontColor(EstiloPdf.AzulOscuro);
                        foreach (var paso in comanda.Pasos)
                        {
                            columna.Item().PaddingLeft(10).Row(fila =>
                            {
                                fila.ConstantItem(18).Text($"{paso.NroPaso}.").SemiBold().FontColor(EstiloPdf.Azul);
                                fila.RelativeItem().Text(paso.Descripcion).FontSize(9.5f);
                            });
                        }
                    }
                });

                pagina.Footer().Element(EstiloPdf.PieDePagina);
            });
        }).GeneratePdf();
    }
}
