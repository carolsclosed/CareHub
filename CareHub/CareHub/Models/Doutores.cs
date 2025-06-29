using System.ComponentModel.DataAnnotations; // Usado para atributos de validação de dados (e.g., Key, RegularExpression).
using System.ComponentModel.DataAnnotations.Schema; // Usado para atributos de mapeamento de esquema da base de dados (e.g., ForeignKey).

namespace CareHub.Models { // Define o namespace para os modelos da aplicação.

    // Representa a entidade 'Doutor' no sistema, mapeada para uma tabela na base de dados.
    public class Doutores {

        [Key] // Define 'IdDoutor' como a chave primária da tabela 'Doutores'.
        public int IdDoutor { get; set; } // ID único do doutor.
        
        [ForeignKey(nameof(Utilizador))] // Define 'IdUtil' como uma chave estrangeira que se relaciona com a entidade 'Utilizador'.
        public int IdUtil { get; set; } // ID do utilizador associado a este doutor.
        
        [RegularExpression(@"^\d{1,6}$")] // Atributo de validação: Garante que 'nCedula' é uma string de 1 a 6 dígitos numéricos.
        public int nCedula  { get; set; } // Número da cédula profissional do doutor.
        
        public string Especialidade { get; set; } // Especialidade médica do doutor.
        
        public string DistritoProfissional { get; set; } // Distrito onde o doutor exerce a profissão.
        
        public string Nome { get; set; } // Nome completo do doutor.
        
        public string email { get; set; } // Endereço de email do doutor.
        public string Descricao { get; set; } // Descrição ou biografia do doutor.
        
        // Propriedade de navegação para a entidade 'Utilizador'.
        // Representa o utilizador (pessoa física no sistema) ao qual este registro de doutor está associado.
        public Utilizadores Utilizador { get; set; } 
    }
}