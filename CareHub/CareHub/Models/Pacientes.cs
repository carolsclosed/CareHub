using System.ComponentModel.DataAnnotations; // Para [Key].
using System.ComponentModel.DataAnnotations.Schema; // Para [ForeignKey].

namespace CareHub.Models { // Modelos da aplicação.

    // Entidade Paciente na base de dados.
    public class Pacientes {

        [Key]  // Chave primária.
        public int IdPaciente { get; set; } // ID do paciente.
        
        [ForeignKey(nameof(Utilizador))] // Chave estrangeira para Utilizador.
        public int IdUtil { get; set; } // ID do utilizador associado.
        
        public Utilizadores Utilizador { get; set; } // Objeto Utilizador relacionado.
    }
}