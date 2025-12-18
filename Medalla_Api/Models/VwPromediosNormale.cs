using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class VwPromediosNormale
{
    public int CandidataId { get; set; }

    public string NombreCandidata { get; set; } = null!;

    public string? FotoUrl { get; set; }

    public int? TotalVotos { get; set; }

    public int? SumaPuntos { get; set; }

    public decimal? Promedio { get; set; }
}
