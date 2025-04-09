using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Utilizadores {

        [Key]  // PK, int, autonumber
        public int id_util { get; set; }

        public string nome { get; set; }

        public string regiao { get; set; }
        
        public string email { get; set; }
        
        public string telefone { get; set; }
        
        [ForeignKey("id_paciente")]
        public int id_paciente { get; set; }
        
        [ForeignKey("id_doutor")]
        public int id_doutor { get; set; }
        


        //public ICollection<Comentarios> ListaComentarios { get; set; }
    }
    
}