using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Pacientes {

        [Key]  // PK, int, autonumber
        public int IdPaciente { get; set; }
        
        [ForeignKey("id_util")]
        public int IdUtil { get; set; }
        

    }
    
}