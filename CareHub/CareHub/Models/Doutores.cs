using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Doutores {

        [Key]
        public int IdDoutor { get; set; }
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }
        
        public Utilizadores Utilizador { get; set; }
        

    }
    
}