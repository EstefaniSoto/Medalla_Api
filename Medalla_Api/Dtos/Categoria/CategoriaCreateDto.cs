using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Categoria
{
    public class CategoriaCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
    }
}
