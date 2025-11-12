using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class SummaryPage : ContentPage
{
	private readonly SummaryViewModel viewModel;
	public SummaryPage(SummaryViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel=viewModel;
		BindingContext=viewModel;

	}
	private async void OnLoaded(object? sender, EventArgs e)
	{
		await viewModel.LoadDataCommand.ExecuteAsync(null); 
	}
}