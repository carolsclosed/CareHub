namespace CareHub.Models; // Modelos da aplicação.

// ViewModel para exibir informações de erro.
public class ErrorViewModel
{
    public string? RequestId { get; set; } // ID da requisição, pode ser null.

    // Verdadeiro se o RequestId tiver um valor e não for vazio; caso contrário, falso.
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}