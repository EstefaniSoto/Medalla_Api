using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos;

namespace Medalla_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidatasController : ControllerBase
{
    private readonly MedallaContext _context;

    public CandidatasController(MedallaContext context)
    {
        _context = context;
    }

    // GET: api/Candidatas
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var candidatas = await _context.Candidatas
            .Include(c => c.Categoria)
            .Select(c => new
            {
                candidataId = c.CandidataId,
                nombre = c.Nombre,
                fotoUrl = c.FotoUrl,
                categoriaId = c.CategoriaId,
                categoriaNombre = c.Categoria.Nombre
            })
            .ToListAsync();

        return Ok(candidatas);
    }

    // POST: api/Candidatas
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CandidataCreateDto dto)
    {
        string? fotoUrl = null;

        if (dto.Foto != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Foto.FileName);
            var path = Path.Combine("wwwroot", "fotos", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

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

    // PUT: api/Candidatas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromForm] CandidataUpdateDto dto)
    {
        var candidata = await _context.Candidatas.FindAsync(id);

        if (candidata == null)
            return NotFound();

        candidata.Nombre = dto.Nombre;
        candidata.CategoriaId = dto.CategoriaId;

        if (dto.Foto != null)
        {
            if (!string.IsNullOrEmpty(candidata.FotoUrl))
            {
                var oldPath = Path.Combine(
                    "wwwroot",
                    candidata.FotoUrl.TrimStart('/')
                );

                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Foto.FileName);
            var newPath = Path.Combine("wwwroot", "fotos", fileName);

            using var stream = new FileStream(newPath, FileMode.Create);
            await dto.Foto.CopyToAsync(stream);

            candidata.FotoUrl = $"/fotos/{fileName}";
        }

        await _context.SaveChangesAsync();

        return Ok(candidata);
    }

    // DELETE: api/Candidatas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var candidata = await _context.Candidatas.FindAsync(id);
        if (candidata == null)
            return NotFound();

        _context.Candidatas.Remove(candidata);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
