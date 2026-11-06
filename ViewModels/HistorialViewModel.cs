using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasAppSQLite.Models;
using NotasAppSQLite.Services;

namespace NotasAppSQLite.ViewModels;

public partial class HistorialViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    public ObservableCollection<Historial> ListaHistorial { get; } = new();

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    public HistorialViewModel(NotasDatabase database)
    {
        _database = database;
    }

    [RelayCommand]
    public async Task CargarHistorialAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var historialDb = await _database.GetHistorialAsync();
            ListaHistorial.Clear();
            foreach (var item in historialDb)
            {
                ListaHistorial.Add(item);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
