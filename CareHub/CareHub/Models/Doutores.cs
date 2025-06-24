using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Doutores {

        [Key]
        public int IdDoutor { get; set; }
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }
        
        [RegularExpression(@"^\d{1,6}$")]
        public int nCedula  { get; set; }
        
        public string Especialidade { get; set; }
        
        public string DistritoProfissional { get; set; }
        
        public string Nome { get; set; }
        
        public string email { get; set; }
        public string Descricao { get; set; }
        
        public Utilizadores Utilizador { get; set; }
        

    }
    
}