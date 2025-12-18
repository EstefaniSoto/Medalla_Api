using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class Finalista
{
    public int FinalistaId { get; set; }

    public int CandidataId { get; set; }

    public virtual Candidata Candidata { get; set; } = null!;
}
