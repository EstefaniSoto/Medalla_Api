namespace Medalla_Api.Dtos.Resultados
{
    public class ResultadoCandidataDto
    {
        public int CandidataId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? FotoUrl { get; set; }

        public int TotalVotos { get; set; }
        public int SumaPuntos { get; set; }
        public decimal Promedio { get; set; }
    }
}
