using CommunityToolkit.Mvvm.Messaging;
using StudyTracker.ViewModels;

namespace StudyTracker;

public partial class SubjectListPage : ContentPage
{
    private readonly SubjectListViewModel subjectListViewModel;
    public SubjectListPage(SubjectListViewModel subjectListViewModel)
    {
        InitializeComponent();
        this.subjectListViewModel = subjectListViewModel;
        BindingContext = this.subjectListViewModel;
        WeakReferenceMessenger.Default.Register<string>(this, async (r, m) =>
        {
            await DisplayAlert("Hiba", m, "OK");
        });
    }
    private async void OnLoaded(object? sender, EventArgs e)
    {
        await subjectListViewModel.LoadSubjectsCommand.ExecuteAsync(null);
    }
}