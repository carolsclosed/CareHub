using System.ComponentModel.DataAnnotations.Schema; // Para [ForeignKey].
using Microsoft.EntityFrameworkCore; // Para [PrimaryKey].

namespace CareHub.Models { // Modelos da aplicação.

    // Representa uma "curtida" (upvote) em um post.
    // Chave primária composta por IdUtil e IdPost, garantindo que um utilizador só curta um post uma vez.
    [PrimaryKey(nameof(IdUtil),nameof(IdPost))]
    public class Up
    {
        // Chaves estrangeiras que ligam a quem curtiu e o que foi curtido.
        [ForeignKey(nameof(Utilizador))]
        public int IdUtil { get; set; }    // ID do utilizador que curtiu.
        
        [ForeignKey(nameof(Post))]
        public int IdPost { get; set; } // ID do post curtido.

        // Propriedades de navegação para os objetos relacionados.
        public Utilizadores Utilizador { get; set; } // O utilizador que curtiu.
        public Posts Post { get; set; } // O post que foi curtido.
    }
}