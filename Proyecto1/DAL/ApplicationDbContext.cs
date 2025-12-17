using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;

namespace Proyecto1.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().HasNoKey(); // Usuarios vía SP

            modelBuilder.Entity<Colaborador>()
                .HasKey(c => c.IdColaborador); // Colaboradores tabla real
        }
    }
}
