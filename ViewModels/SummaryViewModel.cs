using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microcharts;
using SkiaSharp;
using StudyTracker.Data;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
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
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send($"Hiba a diagram betöltésekor: {ex.Message}");
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
                StudyReportDocument generator = new StudyReportDocument(subjectReportData);
                string textContent = generator.GenerateTextReport();

                string generatedFile = Path.Combine(FileSystem.CacheDirectory, "Jegy_Riport.txt");

                await File.WriteAllTextAsync(generatedFile, textContent);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Jegy_Riport.txt",
                    File = new ShareFile(generatedFile, "text/plain") 
                });

            }
            catch (Exception e)
            {

                WeakReferenceMessenger.Default.Send($"Nem sikerült a generálás/fájlmegosztás: {e.Message}");
            }


        }
        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
