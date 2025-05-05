using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Doutores {

        [Key]  // PK, int, autonumber
        public int IdPaciente { get; set; }
        
        [ForeignKey("IdUtil")]
        public int IdUtil { get; set; }
        

    }
    
}