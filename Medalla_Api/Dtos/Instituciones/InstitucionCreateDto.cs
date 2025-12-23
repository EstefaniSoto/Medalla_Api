using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Instituciones;

public class InstitucionCreateDto
{
    public string Nombre { get; set; } = null!;
}
