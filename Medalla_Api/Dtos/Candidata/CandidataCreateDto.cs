namespace Medalla_Api.Dtos.Candidata;

public class CandidataCreateDto
{
    public string Nombre { get; set; } = null!;
    public int CategoriaId { get; set; }
    public IFormFile? Foto { get; set; }
}
