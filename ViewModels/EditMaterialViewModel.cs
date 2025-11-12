using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Storage;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(MaterialToEdit), "MaterialToEdit")]

    public partial class EditMaterialViewModel : ObservableObject
    {
        [ObservableProperty]
        Material materialToEdit;
       
        public EditMaterialViewModel()
        {
            materialToEdit = new();
        }

       

        [RelayCommand]
        async Task TakePhotoAsync()
        {
            try
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    string localFilePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                    using (Stream sourceStream = await photo.OpenReadAsync())
                    using (FileStream localStream = File.OpenWrite(localFilePath))
                    {
                        await sourceStream.CopyToAsync(localStream);
                    }
                    MaterialToEdit.PictureUri = localFilePath;
                }

            }
            catch (PermissionException pEx)
            {
                Debug.WriteLine($"Engedély hiba: {pEx.Message}");
                WeakReferenceMessenger.Default.Send("Hiba: Nincs engedély a kamera használatához.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fotó készítése hiba: {ex.Message}");
                WeakReferenceMessenger.Default.Send("Hiba a fotó készítése közben.");
            }
        }

        [RelayCommand]
        async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(MaterialToEdit.PictureUri) || string.IsNullOrWhiteSpace(MaterialToEdit.Description))
            {
                WeakReferenceMessenger.Default.Send("A kép és a leírás megadása kötelező!");
                return;
            }

            var param = new ShellNavigationQueryParameters
            {
                { "SavedMaterial", MaterialToEdit }
            };
            await Shell.Current.GoToAsync("..", param);
        }
        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }


}
