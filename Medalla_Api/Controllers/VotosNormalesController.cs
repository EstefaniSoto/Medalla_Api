using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos.VotosNormales;

namespace Medalla_Api.Controllers
{
    [ApiController]
    [Route("api/votos-normales")]
    public class VotosNormalesController : ControllerBase
    {
        private readonly MedallaContext _context;

        public VotosNormalesController(MedallaContext context)
        {
            _context = context;
        }

        [HttpGet("existe")]
        public async Task<IActionResult> ExisteVoto(
    [FromQuery] int usuarioId,
    [FromQuery] int candidataId
)
        {
            var existe = await _context.VotosNormales.AnyAsync(v =>
                v.UsuarioId == usuarioId &&
                v.CandidataId == candidataId
            );

            return Ok(existe);
        }
        // POST: api/votos-normales
        [HttpPost]
        public async Task<IActionResult> CrearVoto([FromBody] VotoNormalCreateDto dto)
        {
            // Validaciones básicas
            if (dto.Puntaje < 0 || dto.Puntaje > 10)
                return BadRequest("El puntaje debe estar entre 0 y 10");

            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuarioId == dto.UsuarioId);

            if (!usuarioExiste)
                return NotFound("Usuario no encontrado");

            var candidataExiste = await _context.Candidatas
                .AnyAsync(c => c.CandidataId == dto.CandidataId);

            if (!candidataExiste)
                return NotFound("Candidata no encontrada");

            // Evitar doble voto
            var votoExiste = await _context.VotosNormales.AnyAsync(v =>
                v.UsuarioId == dto.UsuarioId &&
                v.CandidataId == dto.CandidataId
            );

            if (votoExiste)
                return Conflict("Este usuario ya votó por esta candidata");

            var voto = new VotosNormale
            {
                UsuarioId = dto.UsuarioId,
                CandidataId = dto.CandidataId,
                Puntaje = dto.Puntaje,
                Fecha = DateTime.Now
            };

            _context.VotosNormales.Add(voto);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Voto registrado correctamente",
                votoId = voto.VotoId
            });
        }
    }
}
