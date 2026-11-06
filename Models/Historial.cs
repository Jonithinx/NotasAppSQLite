using SQLite;

namespace NotasAppSQLite.Models;

[Table("Historial")]
public class Historial
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Accion { get; set; } = string.Empty; // "Creación", "Edición", "Eliminación"

    public string Detalle { get; set; } = string.Empty; // Título de la nota afectada

    public DateTime FechaHora { get; set; }
}
