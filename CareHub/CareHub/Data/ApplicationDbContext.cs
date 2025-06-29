using CareHub.Models;
using Microsoft.AspNetCore.Identity;
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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // criar os perfis de utilizador da nossa app
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "a", Name = "Administrador", NormalizedName = "ADMINISTRADOR" });

            // criar um utilizador para funcionar como ADMIN
            // função para codificar a password
            var hasher = new PasswordHasher<IdentityUser>();
            // criação do utilizador
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "admin",
                    UserName = "admin@mail.pt",
                    NormalizedUserName = "ADMIN@MAIL.PT",
                    Email = "admin@mail.pt",
                    NormalizedEmail = "ADMIN@MAIL.PT",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hasher.HashPassword(null, "Aa0_aa")
                }
            );

            modelBuilder.Entity<Utilizadores>()
                // Acessa a propriedade 'Foto' da entidade 'Utilizadores'.
                .Property(u => u.Foto)
                // Configura o tipo de coluna na base de dados para a propriedade 'Foto'.
                .HasColumnType("nvarchar(255)"); // ou "TEXT" para SQLite

           //adicionar dados que serão inseridos na base de dados
            modelBuilder.Entity<Utilizadores>().HasData(
                // Cria uma nova instância da entidade 'Utilizadores' com valores predefinidos.
                new Utilizadores
                {
                    IdUtil = 1, // Define o ID único do utilizador como 1.
                    IdentityRole = "Administrator", // Atribui a role "Administrator" a este utilizador 
                    IdentityUserName = "admin@mail.pt", // Define o nome de utilizador do Identity.
                    Nome = "Administrador", // Define o nome de exibição do utilizador.
                    Foto = "/ImagensUtilizadores/user.jpg" // Define o caminho da foto de perfil padrão para este utilizador.
                });
            
            
            // Associar este utilizador à role ADMIN
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "admin", RoleId = "a" });
        
            // relaçao utilizador comentarios
            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.Utilizador) // cada comentario tem um utilizador
                .WithMany(u => u.ListaComentarios) // um utilzador pode ter muitos comentarios
                .HasForeignKey(c => c.IdUtil); // chave estrangeira para o utilizador

        }

        
        // Em termos da base de dados, cada DbSet geralmente mapeia para uma tabela na base de dados.
        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Doutores> Doutores { get; set; }
        public DbSet<Pacientes> Pacientes { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Up> Ups { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Formularios> Formularios { get; set; }
        
    }
}