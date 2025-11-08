using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class EditSubjectPage : ContentPage
{
    public EditSubjectPage(EditSubjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}