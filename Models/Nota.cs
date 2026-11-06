using SQLite;

namespace NotasAppSQLite.Models;

[Table("Nota")]
public class Nota
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100), NotNull]
    public string Titulo { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }
}
