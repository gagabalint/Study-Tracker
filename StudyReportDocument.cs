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

        public string GenerateTextReport()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("StudyTracker - Tantárgyi Riport");
            sb.AppendLine("===============================");
            sb.AppendLine();

            foreach (var data in reportData)
            {
                sb.AppendLine($"Tantárgy: {data.Subject.Name}");

                string gradesText = data.Grades.Any()
                    ? string.Join(", ", data.Grades.Select(g => g.Value))
                    : "Nincsenek jegyek";

                sb.AppendLine($"  Jegyek: {gradesText}");
                sb.AppendLine($"  Átlag: {data.Average:F2}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

}
