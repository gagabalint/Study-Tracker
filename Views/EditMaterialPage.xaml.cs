using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class EditMaterialPage : ContentPage
{
	public EditMaterialPage(EditMaterialViewModel viewModel )
	{
		InitializeComponent();
		BindingContext=viewModel;
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            await DisplayAlert("Hiba", m, "OK");
        });

    }
}