using CommunityToolkit.Mvvm.Messaging;
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
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            await DisplayAlert("Hiba", m, "OK");
        });

    }
    async void OnLoaded(object? sender, EventArgs e)
	{
	await	viewModel.LoadDataCommand.ExecuteAsync(null);
	}
}