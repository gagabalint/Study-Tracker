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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(SavedMaterial), "SavedMaterial")]

    [QueryProperty(nameof(SubjectId), "SubjectId")]
    public partial class MaterialListViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;
        [ObservableProperty]
        ObservableCollection<Material> materials = new();
        [ObservableProperty]
        string subjectName;
        int subjectId;
        [ObservableProperty]
        Material savedMaterial;
        [ObservableProperty]
        Material selectedMaterial;
        public int SubjectId { get => subjectId; set => subjectId = value; }
        public MaterialListViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;
        }
        [RelayCommand]
        void SelectSubject(Material material)
        {
            if (SelectedMaterial != null && material.Id == SelectedMaterial.Id)
                SelectedMaterial = null;
            else
                SelectedMaterial = material;

        }
        async partial void OnSavedMaterialChanged(Material value)
        {
            if (value == null)
                return;
            try
            {
                await database.SaveMaterialAsync(value);
                if (SelectedMaterial != null && SelectedMaterial.Id == value.Id)
                {
                    if (Materials.Where(i => SelectedMaterial.Id == value.Id).Any())
                        Materials.Remove(SelectedMaterial);
                }
                Materials.Add(value);
            }
            catch (Exception ex)
            {
            }
            finally { SelectedMaterial = null; ; }
        }
        [RelayCommand]
        async Task AddNewMaterialAsync()
        {
            if (SubjectId == 0) return;
            try
            {
                SelectedMaterial = null;
                Material materialToEdit = new Material()
                {
                    Date = DateTime.Now,
                    SubjectId = this.SubjectId,
                };
                var param = new ShellNavigationQueryParameters { { "MaterialToEdit", materialToEdit } };
               await Shell.Current.GoToAsync("editmaterial", param);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigációs hiba: {ex.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült megnyitni az oldalt.");
            }
        }
        [RelayCommand]
        async Task EditMaterialAsync()
        {
            if (SelectedMaterial == null)
            {
                WeakReferenceMessenger.Default.Send("Módosításhoz jelölj ki egy tananyagot!");
                return;
            }
            try
            {
                var param = new ShellNavigationQueryParameters { { "MaterialToEdit", SelectedMaterial } };
                await Shell.Current.GoToAsync("editmaterial", param);

            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Navigációs hiba: {ex.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült megnyitni az oldalt.");

            }
        }
        [RelayCommand]
        async Task DeleteMaterialAsync()
        {
            if (SelectedMaterial == null)
            {
                WeakReferenceMessenger.Default.Send("Módosításhoz jelölj ki egy tananyagot!");
                return;
            }
            bool confirmed = await Shell.Current.DisplayAlert("Törlés", "Biztosan törlöd ezt a tananyagot?", "Igen", "Nem");
            if (confirmed)
            {
                try
                {
                    await database.DeleteMaterialAsync(SelectedMaterial);
                    Materials.Remove(SelectedMaterial);
                    SelectedMaterial = null;
                }
                catch (Exception)
                {

                    WeakReferenceMessenger.Default.Send("Hiba: Nem sikerült a törlés.");

                }
            }
        }
        [RelayCommand]
        async Task LoadDataAsync()
        {
            if (subjectId == 0)
                return;
            try
            {
                var subject = await database.GetSubjectAsync(SubjectId);
                var dbMaterials = await database.GetMaterialsForSubjectAsync(SubjectId);

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (subject != null) SubjectName = subject.Name;
                    Materials.Clear();
                    if (dbMaterials != null)
                    {
                        foreach (var dbMaterial in dbMaterials)
                        {
                            Materials.Add(dbMaterial);
                        }
                    }
                });

            }
            catch (Exception ex)
            {//TODO 
            }
        }
        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
