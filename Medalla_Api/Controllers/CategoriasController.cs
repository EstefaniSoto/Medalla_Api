using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos.Candidata;
using Medalla_Api.Dtos.Categoria;

namespace Medalla_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly MedallaContext _context;

        public CategoriasController(MedallaContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<IActionResult> ObtenerCategorias()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return Ok(categorias);
        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound("Categoría no encontrada.");

            return Ok(categoria);
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CandidataCreateDto dto)
        {
            string? fotoUrl = null;

            if (dto.Foto != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Foto.FileName);
                var path = Path.Combine("wwwroot/fotos", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Foto.CopyToAsync(stream);

                fotoUrl = $"/fotos/{fileName}";
            }

            var candidata = new Candidata
            {
                Nombre = dto.Nombre,
                CategoriaId = dto.CategoriaId,
                FotoUrl = fotoUrl
            };

            _context.Candidatas.Add(candidata);
            await _context.SaveChangesAsync();

            return Ok(candidata);
        }


        // PUT: api/Categorias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] CategoriaUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("El nombre es obligatorio.");

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound("Categoría no encontrada.");

            categoria.Nombre = dto.Nombre;
            categoria.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return Ok(categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound("Categoría no encontrada.");

            // Protección básica (si tiene candidatas)
            var enUso = await _context.Candidatas
                .AnyAsync(c => c.CategoriaId == id);

            if (enUso)
                return BadRequest("No se puede eliminar una categoría con candidatas asociadas.");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok("Categoría eliminada correctamente.");
        }
    }
}
