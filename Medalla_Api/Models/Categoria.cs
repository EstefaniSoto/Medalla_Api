using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Candidata> Candidata { get; set; } = new List<Candidata>();
}
