using Medalla_Api.Dtos.VotosInstitucionales;
using Medalla_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/votos-institucionales")]
public class VotosInstitucionalesController : ControllerBase
{
    private readonly MedallaContext _context;

    public VotosInstitucionalesController(MedallaContext context)
    {
        _context = context;
    }

    // 🔹 TOP 3 por categoría (igual que antes)
    [HttpGet("top3/{categoriaId}")]
    public async Task<IActionResult> ObtenerTop3(int categoriaId)
    {
        var top3 = await _context.VwPromediosNormales
            .Where(v => _context.Candidatas.Any(c =>
                c.CandidataId == v.CandidataId &&
                c.CategoriaId == categoriaId
            ))
            .OrderByDescending(v => v.Promedio)
            .Take(3)
            .Select(v => new
            {
                v.CandidataId,
                Nombre = v.NombreCandidata,
                v.FotoUrl,
                Promedio = v.Promedio ?? 0
            })
            .ToListAsync();

        return Ok(top3);
    }

    // 🔹 REGISTRAR VOTO (1 POR CATEGORÍA)
    [HttpPost]
    public async Task<IActionResult> RegistrarVoto(
        [FromBody] VotoInstitucionalDto dto
    )
    {
        // 🔐 VALIDAR SI YA VOTÓ EN ESA CATEGORÍA
        var yaVoto = await _context.VotosInstituciones
    .AnyAsync(v =>
        v.InstitucionId == dto.InstitucionId &&
        _context.Candidatas.Any(c =>
            c.CandidataId == v.CandidataId &&
            c.CategoriaId == dto.CategoriaId
        )
    );

        if (yaVoto)
            return Conflict("Ya votó en esta categoría");


        var voto = new VotosInstitucione
        {
            InstitucionId = dto.InstitucionId,
            CandidataId = dto.CandidataId,
            Fecha = DateTime.Now
        };

        _context.VotosInstituciones.Add(voto);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Voto institucional registrado" });
    }

    // 🔹 PODIO FINAL (TODAS LAS CANDIDATAS + VOTOS INSTITUCIONALES)
    [HttpGet("podio/{categoriaId}")]
    public async Task<IActionResult> ObtenerPodioInstitucional(int categoriaId)
    {
        var podio = await _context.Candidatas
            .Where(c => c.CategoriaId == categoriaId)
            .Select(c => new
            {
                CandidataId = c.CandidataId,
                Nombre = c.Nombre,
                FotoUrl = c.FotoUrl,

                // 👇 CONTAMOS LOS VOTOS (SI NO TIENE → 0)
                Votos = _context.VotosInstituciones
                    .Count(v => v.CandidataId == c.CandidataId)
            })
            .OrderByDescending(x => x.Votos)
            .ThenBy(x => x.Nombre)
            .Take(3)
            .ToListAsync();

        return Ok(podio);
    }



}
