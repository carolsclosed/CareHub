using System.ComponentModel; // Para DisplayName.
using System.ComponentModel.DataAnnotations; // Para validações (Key, Required, StringLength, RegularExpression).

namespace CareHub.Models { // Modelos da aplicação.

    // Entidade principal para um utilizador no sistema.
    public class Utilizadores {
        
        [Key]  // Chave primária.
        public int IdUtil { get; set; } // ID do utilizador.
        
        [Required] // Obrigatório.
        [DisplayName("Nome")] // Nome visível na UI.
        [StringLength(50)] // Nome com até 50 caracteres.
        public string Nome { get; set; } // Nome do utilizador.
        
        [DisplayName("Foto")] // Foto visível na UI.
        public string Foto { get; set; } // Caminho para a foto de perfil.

        [DisplayName("Região")] // Região visível na UI.
        [StringLength(30)] // Região com até 30 caracteres.
        public string Regiao { get; set; } // Região do utilizador.
        
        [StringLength(50)] // Até 50 caracteres.
        public string? IdentityUserName { get; set; } // Nome de utilizador do sistema de autenticação (e.g., email).
        
        [StringLength(50)] // Até 50 caracteres.
        public string? IdentityRole { get; set; } // Papel do utilizador (e.g., "Administrador").
        
        [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$", ErrorMessage = "Não corresponde ao formato português")] // Validação para telefone português.
        [DisplayName("Número de Telefone")] // Nome visível na UI.
        public string Telefone { get; set; } // Número de telefone.
        
        public Doutores? Doutor { get; set; } // Objeto Doutor (se for um médico).
        public Pacientes? Paciente { get; set; } // Objeto Paciente (se for um paciente).

        [DisplayName("Publicações")] // Publicações visível na UI.
        public ICollection<Posts> ListaPosts { get; set; } = new List<Posts>(); // Posts criados pelo utilizador.
        
        public ICollection<Comentarios> ListaComentarios { get; set; } = new List<Comentarios>(); // Comentários feitos pelo utilizador.
        
        public ICollection<Up> ListaUp { get; set; } = []; // Curtidas dadas pelo utilizador.
    }
}