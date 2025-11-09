using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyTracker.ViewModels
{
    [QueryProperty(nameof(GradeToEdit),"GradeToEdit")]
    public partial class EditGradeViewModel:ObservableObject
    {
        [ObservableProperty]
        Grade gradeToEdit;
      
        
        public EditGradeViewModel()
        {
            gradeToEdit = new Grade();
        }

        [RelayCommand]
        async Task SaveGradeAsync()
        {
            if (GradeToEdit.Value < 1 || GradeToEdit.Value > 5)
            {
                WeakReferenceMessenger.Default.Send("Hiba: Az érdemjegy 1 és 5 közötti kell legyen!");
                return;
            }
            var param = new ShellNavigationQueryParameters{ { "SavedGrade",GradeToEdit} };
            await Shell.Current.GoToAsync("..", param);

        }
        [RelayCommand]
        async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
