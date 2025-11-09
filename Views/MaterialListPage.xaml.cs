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
	 async void OnLoaded(object? sender, EventArgs e)
	{
	await	viewModel.LoadDataCommand.ExecuteAsync(null);
	}
}