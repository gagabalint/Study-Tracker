using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class MaterialListPage : ContentPage
{
	private readonly MaterialListViewModel viewModel;
	public MaterialListPage(MaterialListViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
		BindingContext=viewModel;

	}
	private async void OnLoaded(object? sender, Exception e)
	{
		viewModel.LoadDataCommand.ExecuteAsync(null);
	}
}