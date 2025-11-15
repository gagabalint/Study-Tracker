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

       
        [ObservableProperty]
        Subject savedSubject;
        public SubjectListViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;

        }

        [RelayCommand]
        void SelectSubject(Subject subject)
        {
            if (SelectedSubject != null && subject.Id == SelectedSubject.Id)
                SelectedSubject = null;
            else
                SelectedSubject = subject;

        }

        [RelayCommand]
        async Task GoToSummaryAsync()
        {

            try
            {
                await Shell.Current.GoToAsync("summary");
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült az Összegzés oldal megnyitása:{ex.Message}");
            }
        }

        [RelayCommand]
        async Task GoToGradesAsync(Subject subject)
        {
            if (subject == null)
            {
                WeakReferenceMessenger.Default.Send("Hiba: A tantárgy nem található!");
                return;
            }

            try
            {
                var param = new ShellNavigationQueryParameters { { "SubjectId", subject.Id } };
                await Shell.Current.GoToAsync("gradelist", param);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült megnyitni a jegyek oldalt:{ex.Message}");
            }
        }
        [RelayCommand]
        async Task GoToMaterialsAsync(Subject subject)
        {
            if (subject == null)
            {
                WeakReferenceMessenger.Default.Send("A tantárgy nem található!");
                return;
            }

            try
            {
                var param = new ShellNavigationQueryParameters { { "SubjectId", subject.Id } };
                await Shell.Current.GoToAsync("materiallist", param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigációs hiba a tananyagok oldalára: {ex.Message}");
                WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült megnyitni a tananyagok oldalt: {ex.Message}");
            }
        }


        [RelayCommand]
        async Task AddNewSubjectAsync()
        {
            try
            {
                SelectedSubject = null;
                var newSubject = new Subject { Name = string.Empty };
                var param = new ShellNavigationQueryParameters { { "Subject", newSubject } };
                await Shell.Current.GoToAsync("editsubject", param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigációs hiba: {ex.Message}");
                WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült megnyitni a szerkesztő oldalt:{ex.Message}");
            }
        }
        async partial void OnSavedSubjectChanged(Subject value)
        {
            if (value == null)
                return;
            if (SelectedSubject != null)
            {
                await database.SaveSubjectAsync(value);
                var oldSubject = subjects.Where(i => i.Id == SelectedSubject.Id).FirstOrDefault();
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
            if (SelectedSubject == null)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Jelölj ki egy tantárgyat a módosításhoz!");
                return;
            }
            try
            {
                var param = new ShellNavigationQueryParameters { { "Subject", SelectedSubject } };
                await Shell.Current.GoToAsync("editsubject", param);
            }
            catch (Exception e)
            {
                WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült megnyitni a szerkesztő oldalt: {e.Message}");
            }
        }

        [RelayCommand]
        async Task DeleteSubjectAsync()
        {
            if (SelectedSubject == null)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Jelölj ki egy tantárgyat a törléshez!");
                return;
            }

            bool response = await Shell.Current.DisplayAlert("Törlés", $"Biztosan törölni szeretnéd a {SelectedSubject.Name} tantárgyat?", "Igen", "Nem");
            if (response)
            {
                try
                {

                    await database.DeleteSubjectAsync(SelectedSubject);
                    subjects.Remove(SelectedSubject);
                    SelectedSubject = null;

                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Törlési hiba: {e.Message}");
                    WeakReferenceMessenger.Default.Send($"Hiba: Nem sikerült törölni a tantárgyat: {e.Message}");
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
