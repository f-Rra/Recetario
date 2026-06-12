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

        private static void AgregarEncabezado(PdfPTable tabla, string texto, Font fuente)
        {
            PdfPCell celda = new PdfPCell(new Phrase(texto, fuente));
            celda.BackgroundColor = new BaseColor(60, 60, 60);
            celda.Padding = 5;
            tabla.AddCell(celda);
        }
    }
}
