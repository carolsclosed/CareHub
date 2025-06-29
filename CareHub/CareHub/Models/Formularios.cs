using System.ComponentModel; // Para DisplayName.
using System.ComponentModel.DataAnnotations; // Para validações (Key, RegularExpression, EmailAddress).
using System.ComponentModel.DataAnnotations.Schema; // Para ForeignKey.

namespace CareHub.Models; // Modelos da aplicação.

// Entidade para dados de formulários submetidos.
public class Formularios
{
    [Key]  // Chave primária.
    public int IdForm { get; set; } // ID do formulário.
        
    [ForeignKey(nameof(Utilizador))] // Chave estrangeira para o Utilizador.
    public int IdUtil { get; set; } // ID do utilizador que submeteu.
      
    public string nome { get; set; } // Nome do remetente do formulário.
    
    [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$", ErrorMessage = "Não corresponde ao formato português")] // Validação de telefone português.
    [DisplayName("Número de Telefone")] // Nome visível na UI.
    public int telefone { get; set; } // Número de telefone .
    
    [EmailAddress] // Validação de formato de email.
    [DisplayName("Email")] // Nome visível na UI.
    public string email { get; set; } // Endereço de email.
    
    public bool presencial { get; set; } // Indica se é atendimento presencial (true) ou online (false).
    public string regiao { get; set; } // Região associada ao formulário.
    
    public string descricao { get; set; } // Descrição ou detalhes do formulário.

    public Utilizadores Utilizador { get; set; } // Objeto Utilizador relacionado.
}