namespace CareHub.Models; // Modelos da aplicação.

/// <summary>
/// modelo de erro
/// </summary>
public class ErrorViewModel
{
    public string? RequestId { get; set; } 

    
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}