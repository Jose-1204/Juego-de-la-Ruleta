using System.ComponentModel.DataAnnotations;

namespace Ruleta.Server.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; } //Clave Primaria
        public decimal Balance { get; set; } // del usuario
    }
}
