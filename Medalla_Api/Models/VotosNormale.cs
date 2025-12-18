using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class VotosNormale
{
    public int VotoId { get; set; }

    public int UsuarioId { get; set; }

    public int CandidataId { get; set; }

    public int Puntaje { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Candidata Candidata { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
