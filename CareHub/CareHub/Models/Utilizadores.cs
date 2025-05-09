using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Utilizadores {

        [Key]  // PK, int, autonumber
        public int IdUtil { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(30)]
        public string Regiao { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [RegularExpression("^(?:\\+351)?\\s?(?:2\\d{8}|9[1236]\\d{7})$\n")]
        public string Telefone { get; set; }
        
        public Doutores? Doutor { get; set; }
        public Pacientes? Paciente { get; set; }

        
        
    }
    
}