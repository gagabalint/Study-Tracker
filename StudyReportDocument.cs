using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

using StudyTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker
{
    public class StudyReportDocument
    {
        private readonly List<SubjectReportData> reportData;

        public StudyReportDocument(List<SubjectReportData> reportData)
        {
            this.reportData = reportData;
        }

        public byte[] GeneratePdf()
        {
            // Létrehozzuk a PDF dokumentumot
            PdfDocument document = new PdfDocument();
            document.Info.Title = "StudyTracker - Tantárgyi Riport";

            // Hozzáadunk egy oldalt
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Definiáljuk a betűtípusokat (ezek a standard PDF betűtípusok, 
            // így nem kell hozzájuk semmilyen külső fájl, garantáltan működni fognak)
            XFont fontTitle = new XFont("Helvetica", 20, XFontStyle.Bold);
            XFont fontSubject = new XFont("Helvetica", 16, XFontStyle.Bold);
            XFont fontBody = new XFont("Helvetica", 12, XFontStyle.Regular);

            double currentY = 50; // Kezdő Y pozíció (felülről)
            double margin = 40;

            // 1. Cím
            gfx.DrawString("StudyTracker - Tantárgyi Riport", fontTitle, XBrushes.Black,
                new XRect(margin, currentY, page.Width - 2 * margin, 0),
                XStringFormats.TopLeft);

            currentY += 40; // Hely a cím alatt

            // 2. Tantárgyak listázása
            foreach (var data in reportData)
            {
                // Tantárgy neve
                gfx.DrawString(data.Subject.Name, fontSubject, XBrushes.Black,
                    new XRect(margin, currentY, page.Width - 2 * margin, 0),
                    XStringFormats.TopLeft);
                currentY += 20;

                // Jegyek
                string gradesText = data.Grades.Any()
                    ? string.Join(", ", data.Grades.Select(g => g.Value))
                    : "Nincsenek jegyek";

                gfx.DrawString($"Jegyek: {gradesText}", fontBody, XBrushes.Black,
                    new XRect(margin, currentY, page.Width - 2 * margin, 0),
                    XStringFormats.TopLeft);
                currentY += 20;

                // Átlag
                gfx.DrawString($"Átlag: {data.Average:F2}", fontBody, XBrushes.Black,
                    new XRect(margin, currentY, page.Width - 2 * margin, 0),
                    XStringFormats.TopLeft);
                currentY += 30; // Hely a következő tantárgy előtt
            }

            // 3. Mentés byte tömbbe
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false); // A 'false' fontos, hogy ne zárja le a stream-et
                return stream.ToArray();
            }
        }
    }

}
