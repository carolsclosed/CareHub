using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Posts {

        [Key]  // PK, int, autonumber
        public int id_post { get; set; }

        public string foto { get; set; }

        public string texto_post { get; set; }
        
        public string categoria { get; set; }
        
        [ForeignKey("id_util")]
        public int id_util { get; set; }

        //public ICollection<Comentarios> ListaComentarios { get; set; }
    }
    
}