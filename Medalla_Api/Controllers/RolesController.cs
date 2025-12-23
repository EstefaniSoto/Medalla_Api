using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;

namespace Medalla_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly MedallaContext _context;

    public RolesController(MedallaContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var roles = await _context.Roles
            .Select(r => new
            {
                r.RoleId,
                r.Name
            })
            .ToListAsync();

        return Ok(roles);
    }
}
