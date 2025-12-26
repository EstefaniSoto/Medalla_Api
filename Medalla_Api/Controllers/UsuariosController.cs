using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medalla_Api.Models;
using Medalla_Api.Dtos.Usuarios;

namespace Medalla_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly MedallaContext _context;

    public UsuariosController(MedallaContext context)
    {
        _context = context;
    }

    // ===== GET ALL =====
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Institucion)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Username = u.Username,
                RoleId = u.RoleId,
                InstitucionId = u.InstitucionId,
                Institucion = u.Institucion != null ? u.Institucion.Nombre : null
            })
            .ToListAsync();

        return Ok(usuarios);
    }

    // ===== CREATE =====
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UsuarioCreateDto dto)
    {
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Username = dto.Username,
            PasswordHash = dto.Password, 
            RoleId = dto.RoleId,
            InstitucionId = dto.InstitucionId
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(usuario);
    }

    // ===== UPDATE =====
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UsuarioUpdateDto dto)
    {
        if (id != dto.UsuarioId)
            return BadRequest("ID incorrecto");

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        usuario.Nombre = dto.Nombre;
        usuario.Username = dto.Username;
        usuario.RoleId = dto.RoleId;
        usuario.InstitucionId = dto.InstitucionId;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            usuario.PasswordHash = dto.Password; 
        }

        await _context.SaveChangesAsync();
        return Ok(usuario);
    }

    // ===== DELETE ====
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return Ok();
    }

    //Login

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Usuarios
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Username == dto.Username &&
                u.PasswordHash == dto.Password
            );

        if (user == null)
            return Unauthorized("Usuario o contraseña incorrectos");

        return Ok(new
        {
            usuarioId = user.UsuarioId,
            nombre = user.Nombre,
            role = user.Role.Name,
            institucionId = user.InstitucionId
        });
    }

}
