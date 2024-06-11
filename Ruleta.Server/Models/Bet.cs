using System.ComponentModel.DataAnnotations;

namespace Ruleta.Server.Models
{

    public class Bet
    {
        [Key]
        public int BetId { get; set; } // Clave primaria para identificar la apuesta
        public string UserName { get; set; } // Nombre del usuario que realizó la apuesta
        public decimal Amount { get; set; } // Monto de la apuesta
        public string BetType { get; set; }
        public string BetValue { get; set; }
    }

}