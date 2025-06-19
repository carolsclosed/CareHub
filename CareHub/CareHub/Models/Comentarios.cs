using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Comentarios {

        [Key]  // PK, int, autonumber
        public int IdCom { get; set; }
        
        [DisplayName("Texto comentário")]
        [StringLength(250)]
        public string TextoCom { get; set; }
        
        public DateOnly DataCom { get; set; }
        
        [ForeignKey(nameof(Post))]
        public int IdPost { get; set; }
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }
        
        public Posts Post { get; set; }
        public Utilizadores Utilizador { get; set; }
    }
    
}