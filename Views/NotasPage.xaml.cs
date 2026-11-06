using NotasAppSQLite.ViewModels;

namespace NotasAppSQLite.Views;

public partial class NotasPage : ContentPage
{
    private readonly NotasViewModel _viewModel;

    public NotasPage(NotasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarNotasCommand.ExecuteAsync(null);
    }
}
