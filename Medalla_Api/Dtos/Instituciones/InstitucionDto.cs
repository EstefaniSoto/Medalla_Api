using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Instituciones;

public class InstitucionDto
{
    public int InstitucionId { get; set; }
    public string Nombre { get; set; } = null!;
}
