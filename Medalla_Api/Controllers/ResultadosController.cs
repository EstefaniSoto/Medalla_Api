using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos.Resultados;

namespace Medalla_Api.Controllers
{
    [ApiController]
    [Route("api/resultados-normales")]
    public class ResultadosNormalesController : ControllerBase
    {
        private readonly MedallaContext _context;

        public ResultadosNormalesController(MedallaContext context)
        {
            _context = context;
        }

        // GET: api/resultados-normales/categoria/1
        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> ObtenerResultadosPorCategoria(int categoriaId)
        {
            var resultados = await _context.VwPromediosNormales
                .Where(v => _context.Candidatas.Any(c =>
                    c.CandidataId == v.CandidataId &&
                    c.CategoriaId == categoriaId
                ))
                .OrderByDescending(v => v.SumaPuntos)
                .Select(v => new ResultadoCandidataDto
                {
                    CandidataId = v.CandidataId,
                    Nombre = v.NombreCandidata,
                    FotoUrl = v.FotoUrl,
                    TotalVotos = v.TotalVotos ?? 0,
                    SumaPuntos = v.SumaPuntos ?? 0,
                    Promedio = v.Promedio ?? 0
                })
                .ToListAsync();

            return Ok(resultados);
        }
    }
}
