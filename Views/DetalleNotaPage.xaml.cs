using NotasAppSQLite.ViewModels;

namespace NotasAppSQLite.Views;

public partial class DetalleNotaPage : ContentPage
{
    public DetalleNotaPage(DetalleNotaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
