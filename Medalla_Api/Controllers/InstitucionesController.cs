using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos;

namespace Medalla_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstitucionesController : ControllerBase
{
    private readonly MedallaContext _context;

    public InstitucionesController(MedallaContext context)
    {
        _context = context;
    }

    // ===== GET ALL =====
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var instituciones = await _context.Instituciones
            .Select(i => new InstitucionDto
            {
                InstitucionId = i.InstitucionId,
                Nombre = i.Nombre
            })
            .ToListAsync();

        return Ok(instituciones);
    }

    // ===== GET BY ID =====
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var institucion = await _context.Instituciones.FindAsync(id);

        if (institucion == null)
            return NotFound();

        return Ok(new InstitucionDto
        {
            InstitucionId = institucion.InstitucionId,
            Nombre = institucion.Nombre
        });
    }

    // ===== CREATE =====
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InstitucionCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio");

        var institucion = new Institucione
        {
            Nombre = dto.Nombre
        };

        _context.Instituciones.Add(institucion);
        await _context.SaveChangesAsync();

        return Ok(institucion);
    }

    // ===== UPDATE =====
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] InstitucionUpdateDto dto)
    {
        if (id != dto.InstitucionId)
            return BadRequest("ID incorrecto");

        var institucion = await _context.Instituciones.FindAsync(id);

        if (institucion == null)
            return NotFound();

        institucion.Nombre = dto.Nombre;

        await _context.SaveChangesAsync();

        return Ok(institucion);
    }

    // ===== DELETE =====
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var institucion = await _context.Instituciones.FindAsync(id);

        if (institucion == null)
            return NotFound();

        _context.Instituciones.Remove(institucion);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
