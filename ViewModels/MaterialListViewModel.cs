using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.Data;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(SubjectId), "SubjectId")]
    public partial class MaterialListViewModel : ObservableObject
    {
        private readonly IStudyTrackerDatabase database;
        [ObservableProperty]
        ObservableCollection<Material> materials = new();
        [ObservableProperty]
        string subjectName;
        int subjectId;
        public int SubjectId { get => subjectId; set => subjectId = value; }
        public MaterialListViewModel(IStudyTrackerDatabase database)
        {
            this.database = database;
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
          await  Shell.Current.GoToAsync("..");
        }
    }
}
