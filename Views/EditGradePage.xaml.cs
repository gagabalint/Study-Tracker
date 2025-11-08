using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class EditGradePage : ContentPage
{
	public EditGradePage(EditGradeViewModel viewModel)
	{

		InitializeComponent();
		BindingContext=viewModel;
	}
}