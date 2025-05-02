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

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Doutor> Doutores { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Up> Ups { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure optional one-to-one Utilizador-Paciente
            modelBuilder.Entity<Utilizadores>()
                .HasOne<Paciente>()
                .WithOne(p => p.Utilizador)
                .HasForeignKey<Utilizadores>(u => u.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure optional one-to-one Utilizador-Doutor
            modelBuilder.Entity<Utilizadores>()
                .HasOne<Doutor>()
                .WithOne(d => d.Utilizador)
                .HasForeignKey<Utilizadores>(u => u.IdDoutor)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure only one of IdPost or IdComentario is used in Up
            modelBuilder.Entity<Up>()
                .HasOne(u => u.Post)
                .WithMany()
                .HasForeignKey(u => u.IdPost)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Up>()
                .HasOne(u => u.Comentario)
                .WithMany()
                .HasForeignKey(u => u.IdComentario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}