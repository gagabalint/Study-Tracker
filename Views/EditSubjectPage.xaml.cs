using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class EditSubjectPage : ContentPage
{
    public EditSubjectPage(EditSubjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            await DisplayAlert("Hiba", m, "OK");
        });

    }
}