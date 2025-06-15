using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareHub.Models {

    public class Posts {

        [Key]  // PK, int, autonumber
        public int IdPost { get; set; }

        [DisplayName("Titulo")]
        [StringLength(200)]
        public string TituloPost { get; set; }
        
        public string? Foto { get; set; }
        
        [DisplayName("Texto do post")]
        public string TextoPost { get; set; }
        
        [DisplayName("Categoria da doença")]
        [StringLength(30)]
        public string Categoria { get; set; }
        
        [DisplayName("Data da Publicação")]
        public DateOnly DataPost { get; set; }
        
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }

        public Utilizadores Utilizador { get; set; }
        
        public ICollection<Comentarios> ListaComentarios { get; set; } = new List<Comentarios>();

        public ICollection<Up> ListaUp { get; set; } = [];
    }
    
}