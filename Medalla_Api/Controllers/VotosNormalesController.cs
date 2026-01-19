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

        // =============================
        // GET: Verificar si ya existe voto
        // =============================
        [HttpGet("existe")]
        public async Task<IActionResult> ExisteVoto(int usuarioId, int candidataId)
        {
            var voto = await _context.VotosNormales
                .FirstOrDefaultAsync(v => v.UsuarioId == usuarioId && v.CandidataId == candidataId);

            if (voto == null)
                return Ok(null);

            return Ok(new
            {
                votoId = voto.VotoId,
                puntaje = voto.Puntaje,
                fecha = voto.Fecha
            });
        }

        // =============================
        // POST: Crear o editar voto (AUTO)
        // =============================
        [HttpPost]
        public async Task<IActionResult> CrearOEditarVoto([FromBody] VotoNormalCreateDto dto)
        {
            var usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.UsuarioId == dto.UsuarioId);

            if (!usuarioExiste)
                return NotFound("Usuario no encontrado");

            var candidataExiste = await _context.Candidatas
                .AnyAsync(c => c.CandidataId == dto.CandidataId);

            if (!candidataExiste)
                return NotFound("Candidata no encontrada");

            // Buscar voto existente
            var voto = await _context.VotosNormales.FirstOrDefaultAsync(v =>
                v.UsuarioId == dto.UsuarioId &&
                v.CandidataId == dto.CandidataId
            );

            // SI EXISTE -> EDITAR
            if (voto != null)
            {
                voto.Puntaje = dto.Puntaje;
                voto.Fecha = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Voto actualizado correctamente",
                    votoId = voto.VotoId
                });
            }

            // SI NO EXISTE -> CREAR
            var nuevoVoto = new VotosNormale
            {
                UsuarioId = dto.UsuarioId,
                CandidataId = dto.CandidataId,
                Puntaje = dto.Puntaje,
                Fecha = DateTime.Now
            };

            _context.VotosNormales.Add(nuevoVoto);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Voto registrado correctamente",
                votoId = nuevoVoto.VotoId
            });
        }

        // =============================
        // PUT: Editar voto explícitamente
        // =============================
        [HttpPut("{votoId}")]
        public async Task<IActionResult> EditarVoto(int votoId, [FromBody] VotoNormalUpdateDto dto)
        {
            var voto = await _context.VotosNormales.FindAsync(votoId);

            if (voto == null)
                return NotFound("Voto no encontrado");

            voto.Puntaje = dto.Puntaje;
            voto.Fecha = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Voto editado correctamente",
                votoId = voto.VotoId
            });
        }
    }
}
