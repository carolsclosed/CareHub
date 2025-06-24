using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Utilizadores {

        public int Id { get; set; }
        
        [Key]  // PK, int, autonumber
        public int IdUtil { get; set; }
        
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
        
        [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$")]
        public string Telefone { get; set; }
        
        public Doutores? Doutor { get; set; }
        public Pacientes? Paciente { get; set; }

        [DisplayName("Publicações")]
        public ICollection<Posts> ListaPosts { get; set; }
        
        public ICollection<Comentarios> ListaComentarios { get; set; }
        
        public ICollection<Up> ListaUp { get; set; } = [];
    }
    
}