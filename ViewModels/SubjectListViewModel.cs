using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    [QueryProperty(nameof(SavedSubject), "SavedSubject")]
    public partial class SubjectListViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;

        [ObservableProperty]
        ObservableCollection<Subject> subjects = new ObservableCollection<Subject>();

        [ObservableProperty]
        Subject selectedSubject;
        [RelayCommand]
        async void GoToGradesAsync()
        {
            if (selectedSubject == null)
            {
                WeakReferenceMessenger.Default.Send("Jelölj ki egy tantárgyat a jegyek megtekintéséhez!");
                return;
            }

            try
            {
                var param = new ShellNavigationQueryParameters { { "SubjectId", selectedSubject.Id } };
               await Shell.Current.GoToAsync("gradelist", param);
            }
            catch (Exception ex) {
                Debug.WriteLine($"Navigációs hiba a jegyek oldalára: {ex.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült megnyitni a jegyek oldalt.");
            }
        }
        [ObservableProperty]
        Subject savedSubject;
        public SubjectListViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;
        }

        [RelayCommand]
        async Task AddNewSubjectAsync()
        {
            try
            {
                selectedSubject = null;
                var newSubject = new Subject { Name = string.Empty };
                var param = new ShellNavigationQueryParameters { { "Subject", newSubject } };
                await Shell.Current.GoToAsync("editsubject", param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigációs hiba: {ex.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült megnyitni a szerkesztő oldalt.");
            }
        }
        async partial void OnSavedSubjectChanged(Subject value)
        {
            if (value == null)
                return;
            if (selectedSubject != null)
            {
                await database.SaveSubjectAsync(value);
                var oldSubject = subjects.Where(i => i.Id == selectedSubject.Id).FirstOrDefault();
                if (oldSubject != null)
                {
                    subjects.Remove(oldSubject);
                }
                subjects.Add(value);
            }
            else
            {
                await database.SaveSubjectAsync(value);
                subjects.Add(value);
            }


        }

        [RelayCommand]
        async Task EditSubjectAsync()
        {
            if (selectedSubject == null)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Jelölj ki egy tantárgyat a módosításhoz!");
                return;
            }
            try
            {
                var param = new ShellNavigationQueryParameters { { "Subject", selectedSubject } };
                await Shell.Current.GoToAsync("editsubject", param);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Navigációs hiba: {e.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült megnyitni a szerkesztő oldalt.");
            }
        }

        [RelayCommand]
        async Task DeleteSubjectAsync()
        {
            if (selectedSubject == null)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Jelölj ki egy tantárgyat a törléshez!");
                return;
            }

            bool response = await Shell.Current.DisplayAlert("Törlés", $"Biztosan törölni szeretnéd a {selectedSubject.Name} tantárgyat?", "Igen", "Nem");
            if (response)
            {
                try
                {

                    await database.DeleteSubjectAsync(selectedSubject);
                    subjects.Remove(SelectedSubject);
                    selectedSubject = null;

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Törlési hiba: {e.Message}");
                    WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült törölni a tantárgyat!");
                }
            }

        }

        [RelayCommand]
        public async Task LoadSubjectsAsync()
        {
            try
            {
                var dbSubjects = await database.GetSubjectsAsync();
                Subjects.Clear();
                foreach (var subject in dbSubjects)
                {
                    Subjects.Add(subject);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Hiba a tantárgyak betöltésekor:{e.Message}");
            }
        }


    }
}
