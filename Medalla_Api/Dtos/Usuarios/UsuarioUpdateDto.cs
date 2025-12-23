using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Usuarios
{
    public class UsuarioUpdateDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public int? InstitucionId { get; set; }
    }
}
