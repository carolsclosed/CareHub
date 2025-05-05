using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Utilizadores {

        [Key]  // PK, int, autonumber
        public int IdUtil { get; set; }

        public string Nome { get; set; }

        public string Regiao { get; set; }
        
        public string Email { get; set; }
        
        public string Telefone { get; set; }
        
        public Doutores? Doutor { get; set; }
        public Pacientes? Paciente { get; set; }

        
        
    }
    
}