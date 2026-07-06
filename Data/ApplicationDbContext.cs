using Microsoft.EntityFrameworkCore;
using GestaoReservasUni.Models;

namespace GestaoReservasUni.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aqui dizemos ao Entity Framework para criar as tabelas no banco de dados
        public DbSet<ItemReserva> ItensReserva { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
    }
}