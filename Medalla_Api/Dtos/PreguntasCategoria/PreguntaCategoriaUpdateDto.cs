namespace Medalla_Api.Dtos.PreguntasCategoria;

public class PreguntaCategoriaUpdateDto
{
    public int PreguntaId { get; set; }
    public string Texto { get; set; } = null!;
    public int PuntajeMaximo { get; set; }
    public int Orden { get; set; }
    public bool Activa { get; set; }
}
