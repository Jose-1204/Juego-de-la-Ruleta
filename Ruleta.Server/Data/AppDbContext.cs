using Microsoft.EntityFrameworkCore;
using Ruleta.Server.Models;
using System;

namespace Ruleta.Server.Data
{

    public class AppDbContext : DbContext
    {
        // Constructor que recibe opciones de configuración y las pasa a la base clase
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet representa una colección de todas las entidades en el contexto o que se pueden consultar desde la base de datos
        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; } // Agregar el DbSet para Bet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional si es necesario
        }
    }


}
