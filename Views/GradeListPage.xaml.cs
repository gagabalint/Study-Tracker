using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class GradeListPage : ContentPage
{
	private readonly GradeListViewModel viewModel;
	public GradeListPage(GradeListViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel=viewModel;
		BindingContext=viewModel;
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            await DisplayAlert("Hiba", m, "OK");
        });

    }
    private async void OnLoaded(object? sender, EventArgs e)
    {

		await viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}