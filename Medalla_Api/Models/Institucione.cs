using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class Institucione
{
    public int InstitucionId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<VotosInstitucione> VotosInstituciones { get; set; } = new List<VotosInstitucione>();
}
