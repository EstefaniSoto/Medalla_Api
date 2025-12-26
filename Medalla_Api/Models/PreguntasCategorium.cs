using System;
using System.Collections.Generic;

namespace Medalla_Api.Models;

public partial class PreguntasCategorium
{
    public int PreguntaId { get; set; }

    public int CategoriaId { get; set; }

    public string Texto { get; set; } = null!;

    public int PuntajeMaximo { get; set; }

    public int Orden { get; set; }

    public bool Activa { get; set; }
}
