using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Comentarios {

        [Key]  // PK, int, autonumber
        public int id_com { get; set; }
        
        public string texto_com { get; set; }
        
        [ForeignKey("id_util")]
        public int id_util { get; set; }
        
        [ForeignKey("id_post")]
        public int id_post { get; set; }
        
        //public ICollection<Comentarios> ListaComentarios { get; set; }
    }
    
}