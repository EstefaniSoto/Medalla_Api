using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class Candidata
{
    public int CandidataId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? FotoUrl { get; set; }

    public int CategoriaId { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Finalista? Finalista { get; set; }

    public virtual ICollection<VotosInstitucione> VotosInstituciones { get; set; } = new List<VotosInstitucione>();

    public virtual ICollection<VotosNormale> VotosNormales { get; set; } = new List<VotosNormale>();
}
