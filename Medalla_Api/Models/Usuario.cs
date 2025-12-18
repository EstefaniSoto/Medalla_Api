using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public int? InstitucionId { get; set; }

    public virtual Institucione? Institucion { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<VotosNormale> VotosNormales { get; set; } = new List<VotosNormale>();
}
