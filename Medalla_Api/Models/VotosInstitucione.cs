using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class VotosInstitucione
{
    public int VotoInstId { get; set; }

    public int InstitucionId { get; set; }

    public int CandidataId { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Candidata Candidata { get; set; } = null!;

    public virtual Institucione Institucion { get; set; } = null!;
}
