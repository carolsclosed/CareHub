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
    
    [RegularExpression("^(?:(?:\\+|00)351)?\\s?(9[1236]\\d{7}|2\\d{8})$\n")]
    public int telefone { get; set; }
    
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\n")]
    public string email { get; set; }
    
    public bool presencial { get; set; }
    public string regiao { get; set; }
    
    public string descricao { get; set; }

    
    public Utilizadores Utilizador { get; set; } 
}
