using CareHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Doutores> Doutores { get; set; }
        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Up> Ups { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        
    }
}