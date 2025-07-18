using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace CareHub.Models { 

    /// <summary>
    /// modelo dos pacientes
    /// </summary>
    public class Pacientes {

        [Key]  
        public int IdPaciente { get; set; } 
        
        [ForeignKey(nameof(Utilizador))] 
        public int IdUtil { get; set; } 
        
        public Utilizadores Utilizador { get; set; } 
    }
}