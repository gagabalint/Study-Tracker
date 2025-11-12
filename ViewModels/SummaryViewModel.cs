using CommunityToolkit.Mvvm.ComponentModel;
using StudyTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microcharts;
using StudyTracker.Models;
using QuestPDF.Infrastructure;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using CommunityToolkit.Mvvm.Messaging;
using QuestPDF.Fluent;
namespace StudyTracker.ViewModels
{
    public record SubjectReportData(Subject Subject, List<Grade> Grades, double Average);
    public partial class SummaryViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;
        [ObservableProperty]
        Chart subjectChart;
        private List<SubjectReportData> subjectReportData = new List<SubjectReportData>();
        public SummaryViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;
            subjectChart = new BarChart() { Entries = new List<ChartEntry>() };
            QuestPDF.Settings.License = LicenseType.Community;
        }

        [RelayCommand]
        async Task LoadDataAsync()
        {
            try
            {
                var subjects = await database.GetSubjectsAsync();
                List<ChartEntry> entries = new List<ChartEntry>();
                subjectReportData.Clear();
                string[] colors = new string[] { "#512BD4", "#DFD8F7", "#2B0B98", "#D600AA", "#190649" };
                int colorIndex = 0;
                foreach (var subject in subjects)
                {
                    var grades = await database.GetGradesForSubjectAsync(subject.Id);
                    double average = grades.Any() ? grades.Average(i => i.Value) : 0;
                    subjectReportData.Add(new SubjectReportData(subject, grades, average));

                    if (average > 0)
                    {
                        entries.Add(new ChartEntry((float)Math.Round(average, 2)) { Label = subject.Name, ValueLabel = average.ToString("F2"), Color = SKColor.Parse(colors[(colorIndex++) % colors.Length]) });
                    }
                    SubjectChart = new BarChart
                    {
                        Entries = entries,
                        ValueLabelOrientation = Orientation.Horizontal,
                        LabelTextSize = 25,
                        LabelOrientation = Orientation.Horizontal,
                        IsAnimated = true,
                        MinValue = 1,
                        MaxValue = 5
                    };

                }

            }
            catch (Exception)
            {
                //TODO
            }
        }
        [RelayCommand]
        async Task ShareReportAsync()
        {
            if (subjectReportData.Count == 0)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Nincs adat a riport generálásához!");
                return;
            }
            try
            {
                StudyReportDocument document = new StudyReportDocument(subjectReportData);
                byte[] pdfBytes = document.GeneratePdf();
                string generatedFile = Path.Combine(FileSystem.CacheDirectory, "Jegy_Riport.pdf");
                File.WriteAllBytes(generatedFile, pdfBytes);
                await Share.Default.RequestAsync(new ShareFileRequest { Title = "Jegy_Riport", File = new ShareFile(generatedFile, "application/PDF") });

            }
            catch (Exception x)
            {

                WeakReferenceMessenger.Default.Send("Nem sikerült a generálás/fájlmegosztás!");
            }


        }
        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
