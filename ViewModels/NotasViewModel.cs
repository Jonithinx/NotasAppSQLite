using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasAppSQLite.Models;
using NotasAppSQLite.Services;
using NotasAppSQLite.Views;

namespace NotasAppSQLite.ViewModels;

public partial class NotasViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    public ObservableCollection<Nota> Notas { get; } = new();

    [ObservableProperty]
    public partial string TextoBusqueda { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    public NotasViewModel(NotasDatabase database)
    {
        _database = database;
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _ = CargarNotasAsync();
        }
        else
        {
            _ = BuscarAsync();
        }
    }

    [RelayCommand]
    public async Task CargarNotasAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var notasDb = await _database.GetNotasAsync();
            Notas.Clear();
            foreach (var nota in notasDb)
            {
                Notas.Add(nota);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task BuscarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var notasDb = await _database.BuscarNotasPorTituloAsync(TextoBusqueda);
            Notas.Clear();
            foreach (var nota in notasDb)
            {
                Notas.Add(nota);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CrearNotaAsync()
    {
        await Shell.Current.GoToAsync(nameof(DetalleNotaPage));
    }

    [RelayCommand]
    public async Task EditarNotaAsync(Nota? nota)
    {
        if (nota is null) return;

        await Shell.Current.GoToAsync($"{nameof(DetalleNotaPage)}?id={nota.Id}");
    }

    [RelayCommand]
    public async Task EliminarNotaAsync(Nota? nota)
    {
        if (nota is null) return;

        bool confirmar = await Shell.Current.DisplayAlertAsync(
            "Confirmación",
            "¿Desea eliminar esta nota?",
            "Sí",
            "Cancelar");

        if (!confirmar) return;

        await _database.DeleteNotaAsync(nota);
        await _database.RegistrarAccionAsync("Eliminación", nota.Titulo);
        await CargarNotasAsync();
    }
}
