using System.ComponentModel; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 

namespace CareHub.Models; 

/// <summary>
/// modelo para formularios
/// </summary>
public class Formularios
{
    /// <summary>
    /// Modelo para formularios para marcação de consulta 
    /// </summary>
    [Key]  
    public int IdForm { get; set; } 
        
    [ForeignKey(nameof(Utilizador))] 
    public int IdUtil { get; set; } 
      
    public string nome { get; set; } 
    
    [RegularExpression("^(?:\\+351\\s?)?(9[1236]\\d{7})$", ErrorMessage = "Não corresponde ao formato português")] // Validação de telefone português.
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