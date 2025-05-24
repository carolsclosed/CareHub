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
            .GroupBy(d => NormalizarTexto(d.Categoria))
            .Select(g => g.First())
            .OrderBy(d => d.Categoria)
            .ToList();

        static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            // 1. Remove espaços extras
            texto = texto.Trim();

            // 2. Converte para minúsculas
            texto = texto.ToLowerInvariant();

            // 3. Remove acentos
            texto = texto.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var ch in texto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        
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