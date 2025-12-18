using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class VwPodioFinal
{
    public int CandidataId { get; set; }

    public string NombreCandidata { get; set; } = null!;

    public string? FotoUrl { get; set; }

    public int PuntosInstituciones { get; set; }
}
