using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.ViewModels
{

    [QueryProperty(nameof(SubjectToEdit), "Subject")]
    public partial class EditSubjectViewModel : ObservableObject
    {
        [ObservableProperty]
        Subject subjectToEdit;

        public EditSubjectViewModel()
        {
        }

        [RelayCommand]
        async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(SubjectToEdit?.Name))
            {
                await Shell.Current.DisplayAlert("Hiba", "A tantárgy neve kötelező!", "OK");
                return;
            }

            var param = new ShellNavigationQueryParameters
            {
                { "SavedSubject", SubjectToEdit }
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

