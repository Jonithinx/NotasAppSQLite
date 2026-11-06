using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasAppSQLite.Models;
using NotasAppSQLite.Services;

namespace NotasAppSQLite.ViewModels;

[QueryProperty(nameof(NotaId), "id")]
public partial class DetalleNotaViewModel : ObservableObject, IQueryAttributable
{
    private readonly NotasDatabase _database;

    [ObservableProperty]
    public partial int NotaId { get; set; }

    [ObservableProperty]
    public partial int Id { get; set; }

    [ObservableProperty]
    public partial string Titulo { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Contenido { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorTitulo { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool TieneErrorTitulo { get; set; }

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    public DetalleNotaViewModel(NotasDatabase database)
    {
        _database = database;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj))
        {
            if (idObj is int intId)
            {
                NotaId = intId;
            }
            else if (idObj is string strId && int.TryParse(strId, out var parsedId))
            {
                NotaId = parsedId;
            }
        }
    }

    partial void OnNotaIdChanged(int value)
    {
        if (value > 0)
        {
            _ = CargarNotaAsync(value);
        }
    }

    partial void OnTituloChanged(string value)
    {
        ValidarTitulo();
    }

    private async Task CargarNotaAsync(int notaId)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var nota = await _database.GetNotaByIdAsync(notaId);
            if (nota is not null)
            {
                Id = nota.Id;
                Titulo = nota.Titulo;
                Contenido = nota.Contenido;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    public bool ValidarTitulo()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
        {
            ErrorTitulo = "El título es obligatorio y no puede estar vacío.";
            TieneErrorTitulo = true;
            return false;
        }

        if (Titulo.Trim().Length > 100)
        {
            ErrorTitulo = "El título no puede superar los 100 caracteres.";
            TieneErrorTitulo = true;
            return false;
        }

        ErrorTitulo = string.Empty;
        TieneErrorTitulo = false;
        return true;
    }

    [RelayCommand]
    public async Task GuardarNotaAsync()
    {
        if (!ValidarTitulo())
        {
            await Shell.Current.DisplayAlertAsync("Validación", ErrorTitulo, "Aceptar");
            return;
        }

        if (IsBusy) return;

        try
        {
            IsBusy = true;
            bool esNueva = Id == 0;
            var nota = new Nota
            {
                Id = Id,
                Titulo = Titulo.Trim(),
                Contenido = Contenido?.Trim() ?? string.Empty
            };

            if (!esNueva)
            {
                var existente = await _database.GetNotaByIdAsync(Id);
                nota.FechaCreacion = existente?.FechaCreacion ?? DateTime.Now;
            }

            await _database.SaveNotaAsync(nota);
            await _database.RegistrarAccionAsync(esNueva ? "Creación" : "Edición", nota.Titulo);

            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
