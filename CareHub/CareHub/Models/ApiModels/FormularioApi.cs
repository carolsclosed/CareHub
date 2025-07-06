namespace CareHub.Models.ApiModels;

public class FormularioApi
{
    /// <summary>
    /// formulários de consulta usados pela API
    /// </summary>
    public int IdForm { get; set; }
    public int IdUtilizador { get; set; }
    
    public string Nome { get; set; }
    
    public string Email { get; set; }
    
    public int Telefone { get; set; }
    
    public string Regiao { get; set; }
    
    public string Descricao { get; set; }
    
    public bool presencial { get; set; }
}