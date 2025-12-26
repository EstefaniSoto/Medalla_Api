namespace Medalla_Api.Dtos.PreguntasCategoria;

public class PreguntaCategoriaDto
{
    public int PreguntaId { get; set; }
    public int CategoriaId { get; set; }
    public string Texto { get; set; } = null!;
    public int PuntajeMaximo { get; set; }
    public int Orden { get; set; }
    public bool Activa { get; set; }
}
