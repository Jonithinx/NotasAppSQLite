namespace NotasAppSQLite;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.DetalleNotaPage), typeof(Views.DetalleNotaPage));
    }
}
