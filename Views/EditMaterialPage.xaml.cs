using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class EditMaterialPage : ContentPage
{
	public EditMaterialPage(EditMaterialViewModel viewModel )
	{
		InitializeComponent();
		BindingContext=viewModel;
	}
}