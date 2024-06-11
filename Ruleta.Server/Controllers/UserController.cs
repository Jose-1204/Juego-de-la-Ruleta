using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ruleta.Server.Data;
using Ruleta.Server.Models;

namespace Ruleta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        // Inyección de dependencia para el contexto de base de datos
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("balance/{name}")]
        public async Task<ActionResult<User>> GetBalance(string name)
        {
            // Busca un usuario por nombre
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase));

            if (existingUser != null)
            {
                // Actualiza el balance si el usuario ya existe
                existingUser.Balance = user.Balance;
                _context.Users.Update(existingUser);
            }
            else
            {
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return Ok();
        }
    }
}
