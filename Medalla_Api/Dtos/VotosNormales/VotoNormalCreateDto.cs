namespace Medalla_Api.Dtos.VotosNormales
{
    public class VotoNormalCreateDto
    {
        public int UsuarioId { get; set; }
        public int CandidataId { get; set; }
        public int Puntaje { get; set; }
    }
}
