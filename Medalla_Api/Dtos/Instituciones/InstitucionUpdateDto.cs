using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Instituciones;

public class InstitucionUpdateDto
{
    public int InstitucionId { get; set; }
    public string Nombre { get; set; } = null!;
}
