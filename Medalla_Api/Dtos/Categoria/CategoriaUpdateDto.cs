using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medalla_Api.Dtos.Categoria
{
    public class CategoriaUpdateDto
    {
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
