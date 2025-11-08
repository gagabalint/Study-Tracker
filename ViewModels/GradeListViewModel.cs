using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.Data;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(SubjectId), "SubjectId")]
    public partial class GradeListViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;

        [ObservableProperty]
        ObservableCollection<Grade> grades = new();

        [ObservableProperty]
        string subjectName;

        private int subjectId;
        public int SubjectId
        {
            get => subjectId;
            set
            {
                subjectId = value;

                
            }
        }
        public GradeListViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;
        }
        [RelayCommand]
        async Task LoadDataAsync()
        {
            if (SubjectId == 0)
                return;
            try
            {
                
                var subject = await database.GetSubjectAsync(SubjectId);
                var gradesFromDb = await database.GetGradesForSubjectAsync(SubjectId);

               
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (subject != null)
                    {
                        SubjectName = subject.Name; // UI frissítés
                    }

                    Grades.Clear(); // UI frissítés
                    foreach (var grade in gradesFromDb)
                    {
                        Grades.Add(grade); // UI frissítés
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //weakreference
            }
        }
    }
}
