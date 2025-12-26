namespace Medalla_Api.Dtos.PreguntasCategoria;

public class PreguntaCategoriaCreateDto
{
    public int CategoriaId { get; set; }
    public string Texto { get; set; } = null!;
    public int PuntajeMaximo { get; set; }
    public int Orden { get; set; }
}
