using System.Collections; // Importa o namespace System.Collections, que fornece interfaces e classes que definem coleções de objetos.
using System.Globalization;
using System.Text; // Importa o namespace System.Text, necessário para classes como NormalizationForm .
using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.

// Importa o namespace System.Globalization, necessário para classes como CharUnicodeInfo .


namespace CareHub.Controllers; // Declara o namespace para o controller.
/// <summary>
/// controller para as categorias
/// </summary>
public class Diagnosticos : Controller // Declara a classe Diagnosticos
{
    /// <summary>
    /// Método para mostrar as categorias de doenças
    /// </summary>
    /// <param name="termo"></param>
    /// <returns></returns>
    // GET
    public IActionResult diagnosticos(string termo)
    {
        
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
        
        
        var doencas = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);

        
        if (!string.IsNullOrEmpty(termo))
        {
           
            termo = termo.ToLower();
            
            
            doencas = doencas
                .Where(d => d.Nome.ToLower().Contains(termo) || d.Categoria.ToLower().Contains(termo))
                .ToList(); 
        }
        
        
        doencas = doencas
            .Where(d => !string.IsNullOrWhiteSpace(d.Categoria))   
            .ToList();
        

        
        ViewBag.Termo = termo;
        
        
        return View(doencas);
    }
    
    
    public class InfoDiagnostico
    {
        
        [JsonPropertyName("nome")] 
        public string Nome { get; set; } 

        
        [JsonPropertyName("categoria")] 
        public string Categoria { get; set; } 
        

      
    }
}