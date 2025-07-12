using System.ComponentModel; 
using System.ComponentModel.DataAnnotations; 

namespace CareHub.Models { 

    /// <summary>
    /// modelo dos utilizadores
    /// </summary>
    public class Utilizadores {
        
        [Key] 
        public int IdUtil { get; set; } 
        
        [Required] 
        [DisplayName("Nome")] 
        [StringLength(50)]
        public string Nome { get; set; } 
        
        [DisplayName("Foto")]
        public string Foto { get; set; } 

        [DisplayName("Região")]
        [StringLength(30)] 
        public string Regiao { get; set; } 
        
        [StringLength(50)] 
        public string? IdentityUserName { get; set; } 
        
        [StringLength(50)] 
        public string? IdentityRole { get; set; } 
        
        [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$", ErrorMessage = "Não corresponde ao formato português")] 
        [DisplayName("Número de Telefone")] 
        public string Telefone { get; set; } 
        
        public Doutores? Doutor { get; set; } 
        public Pacientes? Paciente { get; set; }

        [DisplayName("Publicações")] // Publicações visível na UI.
        public ICollection<Posts> ListaPosts { get; set; } = new List<Posts>(); 
        
        public ICollection<Comentarios> ListaComentarios { get; set; } = new List<Comentarios>(); 
        
        public ICollection<Up> ListaUp { get; set; } = []; 
    }
}