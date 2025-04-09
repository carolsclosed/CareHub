using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Pacientes {

        [Key]  // PK, int, autonumber
        public int id_paciente { get; set; }
        
        [ForeignKey("id_util")]
        public int id_util { get; set; }
        
        //public ICollection<Comentarios> ListaComentarios { get; set; }
    }
    
}