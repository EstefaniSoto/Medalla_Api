using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos.PreguntasCategoria;

namespace Medalla_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreguntasCategoriaController : ControllerBase
{
    private readonly MedallaContext _context;

    public PreguntasCategoriaController(MedallaContext context)
    {
        _context = context;
    }

    // 🔹 GET: api/PreguntasCategoria/categoria/5
    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> GetByCategoria(int categoriaId)
    {
        var preguntas = await _context.PreguntasCategoria
            .Where(p => p.CategoriaId == categoriaId && p.Activa)
            .OrderBy(p => p.Orden)
            .Select(p => new PreguntaCategoriaDto
            {
                PreguntaId = p.PreguntaId,
                CategoriaId = p.CategoriaId,
                Texto = p.Texto,
                PuntajeMaximo = p.PuntajeMaximo,
                Orden = p.Orden,
                Activa = p.Activa
            })
            .ToListAsync();

        return Ok(preguntas);
    }

    // 🔹 POST: api/PreguntasCategoria
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PreguntaCategoriaCreateDto dto)
    {
        // 🔒 Máximo 10 preguntas por categoría
        var cantidad = await _context.PreguntasCategoria
            .CountAsync(p => p.CategoriaId == dto.CategoriaId);

        if (cantidad >= 10)
            return BadRequest("La categoría ya tiene el máximo de 10 preguntas.");

        // 🔒 Validar suma de puntajes ≤ 50
        var sumaActual = await _context.PreguntasCategoria
            .Where(p => p.CategoriaId == dto.CategoriaId)
            .SumAsync(p => p.PuntajeMaximo);

        if (sumaActual + dto.PuntajeMaximo > 50)
            return BadRequest("La suma de los puntajes no puede exceder 50.");

        var pregunta = new PreguntasCategorium
        {
            CategoriaId = dto.CategoriaId,
            Texto = dto.Texto,
            PuntajeMaximo = dto.PuntajeMaximo,
            Orden = dto.Orden,
            Activa = true
        };

        _context.PreguntasCategoria.Add(pregunta);
        await _context.SaveChangesAsync();

        return Ok(pregunta);
    }

    // 🔹 PUT: api/PreguntasCategoria/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PreguntaCategoriaUpdateDto dto)
    {
        if (id != dto.PreguntaId)
            return BadRequest("ID incorrecto.");

        var pregunta = await _context.PreguntasCategoria.FindAsync(id);
        if (pregunta == null)
            return NotFound();

        // 🔒 Validar suma excluyendo esta pregunta
        var sumaOtras = await _context.PreguntasCategoria
            .Where(p => p.CategoriaId == pregunta.CategoriaId && p.PreguntaId != id)
            .SumAsync(p => p.PuntajeMaximo);

        if (sumaOtras + dto.PuntajeMaximo > 50)
            return BadRequest("La suma de los puntajes no puede exceder 50.");

        pregunta.Texto = dto.Texto;
        pregunta.PuntajeMaximo = dto.PuntajeMaximo;
        pregunta.Orden = dto.Orden;
        pregunta.Activa = dto.Activa;

        await _context.SaveChangesAsync();
        return Ok(pregunta);
    }

    // 🔹 DELETE lógico
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pregunta = await _context.PreguntasCategoria.FindAsync(id);
        if (pregunta == null)
            return NotFound();

        pregunta.Activa = false;
        await _context.SaveChangesAsync();

        return Ok();
    }
}
