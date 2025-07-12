namespace CareHub.Models.ApiModels;

/// <summary>
/// Modelo de publicações da API
/// </summary>
public class PublicacoesApi
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    
    public string Categoria { get; set; }
    
    public string Texto { get; set; }
    
    public DateOnly DataPub { get; set; }
    
    public string Foto { get; set; }
}