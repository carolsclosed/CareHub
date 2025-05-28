using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;                // <- necessário para NormalizationForm
using System.Globalization;       // <- necessário para CharUnicodeInfo


namespace CareHub.Controllers;

public class Diagnosticos : Controller
{
   
    // GET
    public IActionResult diagnosticos()
    {
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/doenças.json");
        var doencas = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);

        doencas = doencas
            .Where(d => !string.IsNullOrWhiteSpace(d.Categoria))   // remove categorias vazias ou nulas
            .ToList();
        

        
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