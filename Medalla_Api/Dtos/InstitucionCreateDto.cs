using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos;

public class InstitucionCreateDto
{
    public string Nombre { get; set; } = null!;
}
