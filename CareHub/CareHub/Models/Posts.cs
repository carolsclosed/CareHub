using System.ComponentModel; // Para DisplayName.
using System.ComponentModel.DataAnnotations; // Para Key, StringLength.
using System.ComponentModel.DataAnnotations.Schema; // Para ForeignKey.

namespace CareHub.Models { // Modelos da aplicação.

    // Entidade para uma publicação (post).
    public class Posts {

        [Key]  // Chave primária.
        public int IdPost { get; set; } // ID do post.

        [DisplayName("Titulo")] // Título visível na UI.
        [StringLength(200)] // Título com até 200 caracteres.
        public string TituloPost { get; set; } // O título da publicação.
        
        public string? Foto { get; set; } // Caminho para a foto (opcional).
        
        [DisplayName("Texto da Publicação")] // Texto visível na UI.
        public string TextoPost { get; set; } // Conteúdo principal do post.
        
        [DisplayName("Categoria da doença")] // Categoria visível na UI.
        [StringLength(30)] // Categoria com até 30 caracteres.
        public string Categoria { get; set; } // Categoria da doença relacionada.
        
        [DisplayName("Data da Publicação")] // Data visível na UI.
        public DateOnly DataPost { get; set; } // Data da publicação.
        
        [ForeignKey(nameof(Utilizador))] // Chave estrangeira para o Utilizador.
        public int IdUtil { get; set; } // ID do utilizador autor.

        public Utilizadores Utilizador { get; set; } // Objeto Utilizador do autor.
        
        public ICollection<Comentarios> ListaComentarios { get; set; } = new List<Comentarios>(); // Lista de comentários do post.

        public ICollection<Up> ListaUp { get; set; } = []; // Lista de "curtidas" (ups) do post.
    }
}