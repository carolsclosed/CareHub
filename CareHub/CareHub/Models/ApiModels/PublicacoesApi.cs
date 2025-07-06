namespace CareHub.Models.ApiModels;

public class PublicacoesApi
{
    public string Titulo { get; set; }
    
    public string Categoria { get; set; }
    
    public string Texto { get; set; }
    
    public DateOnly DataPub { get; set; }
    
    public string Foto { get; set; }
}