using NotasAppSQLite.ViewModels;

namespace NotasAppSQLite.Views;

public partial class HistorialPage : ContentPage
{
    private readonly HistorialViewModel _viewModel;

    public HistorialPage(HistorialViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarHistorialCommand.ExecuteAsync(null);
    }
}
