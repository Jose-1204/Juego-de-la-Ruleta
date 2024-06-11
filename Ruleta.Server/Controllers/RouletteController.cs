using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ruleta.Server.Data;
using Ruleta.Server.Models;

namespace Ruleta.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RouletteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("bet")]
        public async Task<IActionResult> PlaceBet([FromBody] Bet bet)
        {
            Console.WriteLine($"Received bet from user: {bet.UserName}, Amount: {bet.Amount}, BetType: {bet.BetType}, BetValue: {bet.BetValue}");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == bet.UserName.ToLower());

            if (user == null)
            {
                return NotFound("User not found");
            }

            Random random = new Random();
            int number = random.Next(0, 37); // Genera un número aleatorio entre 0 y 36
            string color = number % 2 == 0 ? "Red" : "Black"; // Determina el color basado en el número
            decimal prizeAmount = 0;

            if (bet.BetType == "Color" && bet.BetValue.Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = bet.Amount * 0.5m;
            }
            else if (bet.BetType == "Number" && bet.BetValue.Equals(number.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = bet.Amount * 36;
            }
            else if (bet.BetType == "EvenOdd" && (bet.BetValue.Equals("Even", StringComparison.OrdinalIgnoreCase) && number % 2 == 0) || (bet.BetValue.Equals("Odd", StringComparison.OrdinalIgnoreCase) && number % 2 != 0))
            {
                prizeAmount = bet.Amount * 1;
            }
            else if (bet.BetType == "NumberColor" && bet.BetValue.Split('_')[0].Equals(number.ToString(), StringComparison.OrdinalIgnoreCase) && bet.BetValue.Split('_')[1].Equals(color, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = bet.Amount * 3;
            }
            else
            {
                user.Balance -= bet.Amount;
            }

            user.Balance += prizeAmount;
            await _context.SaveChangesAsync();

            return Ok(new { PrizeAmount = prizeAmount, Number = number, Color = color });
        }

        [HttpPost("saveuser")]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == user.Name.ToLower());

            if (existingUser != null)
            {
                existingUser.Balance += user.Balance;
            }
            else
            {
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("processBet")]
        public async Task<IActionResult> ProcessBet([FromBody] BetRequest betRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == betRequest.UserName.ToLower());

            if (user == null)
            {
                return NotFound("User not found");
            }

            int prizeAmount = 0;

            if (betRequest.BetType == "Number" && int.Parse(betRequest.BetValue) == betRequest.WinningNumber)
            {
                prizeAmount = betRequest.Amount * 35;
            }
            else if (betRequest.BetType == "Color" && betRequest.BetValue.Equals(betRequest.WinningColor, StringComparison.OrdinalIgnoreCase))
            {
                prizeAmount = betRequest.Amount * 2;
            }

            user.Balance += prizeAmount;
            await _context.SaveChangesAsync();

            var response = new BetResponse
            {
                Number = betRequest.WinningNumber,
                Color = betRequest.WinningColor,
                PrizeAmount = prizeAmount
            };

            return Ok(response);
        }
    }

    
}
