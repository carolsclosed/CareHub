using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Comentarios {

        [Key]  // PK, int, autonumber
        public int IdCom { get; set; }
        
        public string TextoCom { get; set; }
        
        [ForeignKey(nameof(Post))]
        public int IdPost { get; set; }
        public Posts Post { get; set; }
        
        [ForeignKey("IdUtil")]
        public int IdUtil { get; set; }
        
        
    }
    
}