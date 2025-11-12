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
using Microsoft.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.Messaging;


namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(SubjectId), "SubjectId")]
    [QueryProperty(nameof(SavedGrade), "SavedGrade")]
    public partial class GradeListViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;

        [ObservableProperty]
        ObservableCollection<Grade> grades = new();

        [ObservableProperty]
        string subjectName;
        [ObservableProperty]
        Grade savedGrade;
        [ObservableProperty]
        Grade selectedGrade;


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

        async partial void OnSavedGradeChanged(Grade value)
        {
            if (value == null)
                return;
            try
            {
                await database.SaveGradeAsync(value);

                if (SelectedGrade != null && SelectedGrade.Id == value.Id)
                {
                    var oldGrade = Grades.FirstOrDefault(g => g.Id == value.Id);
                    if (oldGrade != null)
                    {
                        Grades.Remove(oldGrade);
                    }
                    Grades.Add(value);
                }

                else 
                {
                    
                    Grades.Add(value);
                }
            }
            catch (Exception ex) { /* ... (hibakezelés) ... */ }
            finally
            {
                SelectedGrade = null;
            }

        }
        [RelayCommand]
        void SelectSubject(Grade grade)
        {
            if (SelectedGrade != null && grade.Id == SelectedGrade.Id)
                SelectedGrade = null;
            else
                SelectedGrade = grade;

        }
        [RelayCommand]
        async Task AddNewGradeAsync()
        {
            if (SubjectId == 0) return;
            try
            {

                SelectedGrade = null;

                var newGrade = new Grade
                {
                    SubjectId = this.SubjectId,
                    Date = DateTime.Today,
                    Value = 5
                };

                var param = new ShellNavigationQueryParameters
                {
                    { "GradeToEdit", newGrade }
                };

                await Shell.Current.GoToAsync("editgrade", param);
            }

            catch (Exception ex) { WeakReferenceMessenger.Default.Send("Navigációs hiba a jegyek oldalra"); }
        }
        [RelayCommand]
        async Task EditGradeAsync()
        {
            if (SelectedGrade == null)
            {
                WeakReferenceMessenger.Default.Send("Navigációs hiba a jegyek oldalra");
                return;
            }

            try
            {
                var param = new ShellNavigationQueryParameters
                {
                    { "GradeToEdit", SelectedGrade }
                };

                await Shell.Current.GoToAsync("editgrade", param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigációs hiba: {ex.Message}");
            }
        }
        [RelayCommand]
        async Task DeleteGradeAsync()
        {
            if (SelectedGrade == null)
            {
                await Shell.Current.DisplayAlert("Hiba", "Nincs jegy kiválasztva a törléshez.", "OK");
                return;
            }

            bool confirmed = await Shell.Current.DisplayAlert(
                "Törlés",
                $"Biztosan törlöd ezt a jegyet: {SelectedGrade.Value}?",
                "Igen", "Nem");

            if (confirmed)
            {
                try
                {
                    await database.DeleteGradeAsync(SelectedGrade);
                    Grades.Remove(SelectedGrade);
                    SelectedGrade = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Törlési hiba: {ex.Message}");
                }
            }
        }
        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
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
                        SubjectName = subject.Name;
                    }

                    Grades.Clear();
                    foreach (var grade in gradesFromDb)
                    {
                        Grades.Add(grade);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült betölteni a Jegyek oldalt.");

            }
        }
    }
}

