using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using StudyTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker
{
    public class StudyReportDocument : IDocument
    {
        private readonly List<SubjectReportData> reportData;

        public StudyReportDocument(List<SubjectReportData> reportData)
        {
            this.reportData = reportData;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50); 

                    
                    page.Header()
                        .Text("StudyTracker - Tantárgyi Riport")
                        .SemiBold().FontSize(20)/*.FontColor(QuestPDF.Infrastructure.Color.FromRGB(0,0,0))*/;

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Spacing(15); 

                            
                            foreach (var data in reportData)
                            {
                                col.Item().Border(1).Padding(10).Column(subjectCol =>
                                {
                                    subjectCol.Item().Text(data.Subject.Name).Bold().FontSize(16);
                                    subjectCol.Item().PaddingBottom(5);

                                    string gradesText = data.Grades.Any()
                                        ? string.Join(", ", data.Grades.Select(g => g.Value))
                                        : "Nincsenek jegyek";

                                    subjectCol.Item().Text($"Jegyek: {gradesText}");

                                    subjectCol.Item().PaddingTop(5);

                                    subjectCol.Item().Text($"Átlag: {data.Average:F2}").SemiBold();
                                });
                            }
                        });

                    //page.Footer()
                    //    .AlignCenter()
                    //    .Text(x =>
                    //    {
                    //        x.Span("Generálva: ");
                    //        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    //    });
                });
        }
    }
}
