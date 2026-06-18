using System;
using System.Collections.Generic;
using System.IO;
using Dominio;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Presentacion.Helpers
{
    public class SeccionComanda
    {
        public string NombreReceta { get; set; }
        public string Sector { get; set; }
        public string Responsable { get; set; }
        public List<IngredienteReceta> Ingredientes { get; set; }
        public List<Procedimiento> Procedimientos { get; set; }
        public List<Modificacion> Modificaciones { get; set; }
    }

    public static class GeneradorPDF
    {
        public static void GenerarComanda(string ruta, int comensales, List<SeccionComanda> secciones)
        {
            Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Font fontSub = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            Font fontReceta = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            Font fontDato = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            Font fontSeccion = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            Font fontCelda = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);

            using (FileStream fs = new FileStream(ruta, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                doc.Add(new Paragraph("Comanda de Cocina", fontTitulo));
                doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}     Comensales: {comensales}", fontSub));
                doc.Add(new Paragraph(" "));

                foreach (SeccionComanda seccion in secciones)
                {
                    doc.Add(new Paragraph(seccion.NombreReceta, fontReceta));
                    doc.Add(new Paragraph($"Sector: {seccion.Sector}     Responsable: {seccion.Responsable}", fontDato));
                    doc.Add(new Paragraph(" "));

                    PdfPTable tabla = new PdfPTable(3);
                    tabla.WidthPercentage = 100;
                    tabla.SetWidths(new float[] { 4f, 1.5f, 1f });

                    AgregarEncabezado(tabla, "Ingrediente", fontHeader);
                    AgregarEncabezado(tabla, "Cantidad", fontHeader);
                    AgregarEncabezado(tabla, "Unidad", fontHeader);

                    foreach (IngredienteReceta ing in seccion.Ingredientes)
                    {
                        tabla.AddCell(new Phrase(ing.NombreIngrediente, fontCelda));
                        tabla.AddCell(new Phrase(ing.CantNeta.ToString("0.##"), fontCelda));
                        tabla.AddCell(new Phrase(ing.Abreviatura, fontCelda));
                    }

                    doc.Add(tabla);
                    doc.Add(new Paragraph(" "));

                    if (seccion.Modificaciones != null && seccion.Modificaciones.Count > 0)
                    {
                        doc.Add(new Paragraph("Modificaciones:", fontSeccion));

                        iTextSharp.text.List cambios = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED);
                        foreach (Modificacion mod in seccion.Modificaciones)
                        {
                            cambios.Add(new ListItem(DescribirModificacion(mod), fontCelda));
                        }
                        doc.Add(cambios);
                        doc.Add(new Paragraph(" "));
                    }

                    if (seccion.Procedimientos != null && seccion.Procedimientos.Count > 0)
                    {
                        doc.Add(new Paragraph("Procedimiento:", fontSeccion));

                        iTextSharp.text.List pasos = new iTextSharp.text.List(iTextSharp.text.List.ORDERED);
                        foreach (Procedimiento paso in seccion.Procedimientos)
                        {
                            pasos.Add(new ListItem(paso.Descripcion, fontCelda));
                        }
                        doc.Add(pasos);
                        doc.Add(new Paragraph(" "));
                    }
                }

                doc.Close();
            }
        }

        public static void GenerarCosto(string ruta, string nombreReceta, int porciones, List<DetalleCosto> detalle, decimal costoTotal, decimal costoUnitario)
        {
            Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Font fontSub = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            Font fontReceta = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            Font fontCelda = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            Font fontTotal = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);

            using (FileStream fs = new FileStream(ruta, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                doc.Add(new Paragraph("Costo de Receta", fontTitulo));
                doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}", fontSub));
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph(nombreReceta, fontReceta));
                doc.Add(new Paragraph(" "));

                PdfPTable tabla = new PdfPTable(4);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 4f, 2f, 2f, 2f });

                AgregarEncabezado(tabla, "Ingrediente", fontHeader);
                AgregarEncabezado(tabla, "Cant. bruta", fontHeader);
                AgregarEncabezado(tabla, "Precio unit.", fontHeader);
                AgregarEncabezado(tabla, "Costo", fontHeader);

                foreach (DetalleCosto d in detalle)
                {
                    tabla.AddCell(new Phrase(d.NombreIngrediente, fontCelda));
                    tabla.AddCell(new Phrase($"{d.CantBruta:0.##} {d.Unidad}", fontCelda));
                    tabla.AddCell(new Phrase($"${d.CostoUnitario:N2}", fontCelda));
                    tabla.AddCell(new Phrase($"${d.CostoIngrediente:N2}", fontCelda));
                }

                doc.Add(tabla);
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Costo total: ${costoTotal:N2}", fontTotal));
                doc.Add(new Paragraph($"Porciones: {porciones}     Costo por porción: ${costoUnitario:N2}", fontSub));

                doc.Close();
            }
        }

        private static string DescribirModificacion(Modificacion m)
        {
            switch (m.Tipo)
            {
                case "sustitucion":
                    return $"Sustituir {m.IngredienteOriginal} por {m.IngredienteReemplazo}";
                case "adicion":
                    return $"Agregar {m.IngredienteReemplazo}";
                case "eliminacion":
                    return $"Quitar {m.IngredienteOriginal}";
                default:
                    return m.Tipo;
            }
        }

        private static void AgregarEncabezado(PdfPTable tabla, string texto, Font fuente)
        {
            PdfPCell celda = new PdfPCell(new Phrase(texto, fuente));
            celda.BackgroundColor = new BaseColor(60, 60, 60);
            celda.Padding = 5;
            tabla.AddCell(celda);
        }
    }
}
