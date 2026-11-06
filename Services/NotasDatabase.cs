using SQLite;
using NotasAppSQLite.Models;

namespace NotasAppSQLite.Services;

public class NotasDatabase
{
    private SQLiteAsyncConnection? _database;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    private async Task Init()
    {
        if (_database is not null)
            return;

        await _initLock.WaitAsync();
        try
        {
            if (_database is not null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "notas_app.db3");
            _database = new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            await _database.CreateTableAsync<Nota>();
            await _database.CreateTableAsync<Historial>();
        }
        finally
        {
            _initLock.Release();
        }
    }

    // ==========================================
    // Métodos CRUD para Nota
    // ==========================================

    public async Task<List<Nota>> GetNotasAsync()
    {
        await Init();
        return await _database!.Table<Nota>()
                               .OrderByDescending(n => n.FechaModificacion)
                               .ToListAsync();
    }

    public async Task<List<Nota>> BuscarNotasPorTituloAsync(string query)
    {
        await Init();
        if (string.IsNullOrWhiteSpace(query))
        {
            return await GetNotasAsync();
        }

        var param = $"%{query.Trim()}%";
        return await _database!.QueryAsync<Nota>(
            "SELECT * FROM [Nota] WHERE [Titulo] LIKE ? ORDER BY [FechaModificacion] DESC",
            param);
    }

    public async Task<Nota?> GetNotaByIdAsync(int id)
    {
        await Init();
        return await _database!.Table<Nota>()
                               .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<int> SaveNotaAsync(Nota nota)
    {
        await Init();
        if (nota.Id != 0)
        {
            nota.FechaModificacion = DateTime.Now;
            return await _database!.UpdateAsync(nota);
        }
        else
        {
            nota.FechaCreacion = DateTime.Now;
            nota.FechaModificacion = DateTime.Now;
            return await _database!.InsertAsync(nota);
        }
    }

    public async Task<int> DeleteNotaAsync(Nota nota)
    {
        await Init();
        return await _database!.DeleteAsync(nota);
    }

    // ==========================================
    // Métodos para Historial
    // ==========================================

    public async Task<int> RegistrarAccionAsync(string accion, string detalle)
    {
        await Init();
        var historial = new Historial
        {
            Accion = accion,
            Detalle = detalle,
            FechaHora = DateTime.Now
        };
        return await _database!.InsertAsync(historial);
    }

    public async Task<List<Historial>> GetHistorialAsync()
    {
        await Init();
        return await _database!.Table<Historial>()
                               .OrderByDescending(h => h.FechaHora)
                               .ToListAsync();
    }
}
