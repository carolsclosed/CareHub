using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Posts {

        [Key]  // PK, int, autonumber
        public int IdPost { get; set; }

        public string TituloPost { get; set; }
        
        public string? Foto { get; set; }

        public string TextoPost { get; set; }
        
        public string Categoria { get; set; }
        
        
        public DateOnly DataPost { get; set; }
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }

        public Utilizadores Utilizador { get; set; }
    }
    
}