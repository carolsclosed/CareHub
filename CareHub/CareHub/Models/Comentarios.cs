using System.ComponentModel; // Para atributos como DisplayName.
using System.ComponentModel.DataAnnotations; // Para atributos como Key, StringLength.
using System.ComponentModel.DataAnnotations.Schema; // Para atributos como ForeignKey.

namespace CareHub.Models { // Define o namespace para os modelos.

    // Representa um comentário no sistema, mapeado para uma tabela na base de dados.
    public class Comentarios {

        [Key]  // Define IdCom como a chave primária.
        public int IdCom { get; set; } // ID único do comentário.
        
        [DisplayName("Texto comentário")] // Nome de exibição para a UI.
        [StringLength(250)] // Limita o texto a 250 caracteres.
        public string TextoCom { get; set; } // Conteúdo do comentário.
        
        public DateOnly DataCom { get; set; } // Data do comentário (apenas data).
        
        [ForeignKey(nameof(Post))] // Chave estrangeira para o Post.
        public int IdPost { get; set; } // ID do Post associado.
        
        [ForeignKey(nameof(Utilizador))] // Chave estrangeira para o Utilizador.
        public int IdUtil { get; set; } // ID do Utilizador que fez o comentário.
        
        // Propriedades de navegação para aceder os objetos relacionados.
        public Posts Post { get; set; } // O Post ao qual o comentário pertence.
        public Utilizadores Utilizador { get; set; } // O Utilizador que fez o comentário.
    }
}