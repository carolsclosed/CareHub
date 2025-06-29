using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Sqlite.Query.SqlExpressions.Internal;
using System.ComponentModel.DataAnnotations;


namespace CareHub.Models;

public class Formularios
{
    
    [Key]  // PK, int, autonumber
    public int IdForm { get; set; }
        
   [ForeignKey(nameof(Utilizador))]
   public int IdUtil { get; set; }
      
    public string nome { get; set; }
    
    [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$", ErrorMessage = "Não corresponde ao formato português")]
    [DisplayName("Número de Telefone")]
    public int telefone { get; set; }
    
    [EmailAddress]
    [DisplayName("Email")]
    public string email { get; set; }
    
    public bool presencial { get; set; }
    public string regiao { get; set; }
    
    public string descricao { get; set; }

    
    public Utilizadores Utilizador { get; set; } 
}
