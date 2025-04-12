using Microsoft.EntityFrameworkCore;
using AppGestorResumo.Models;

namespace AppGestorResumo.Data
{
    public class SambaDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=SEU_SERVIDOR;Database=SambaPOS;User Id=USUARIO;Password=SENHA;");
        }
    }
}
