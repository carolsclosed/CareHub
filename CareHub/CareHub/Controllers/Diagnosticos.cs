using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CareHub.Controllers;

public class Diagnosticos : Controller
{
   
    // GET
    public IActionResult diagnosticos()
    {
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/diagnosticos.json");
        var conteudo = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);


        
        return View(conteudo);
    }
    
    public class InfoDiagnostico
    {
        public string KeyId { get; set; }
        [JsonPropertyName("primary_name")] 
        public string PrimaryName { get; set; }
        public string ConsumerName { get; set; }
        public bool IsProcedure { get; set; }
        public List<string> Synonyms { get; set; }
        public string WordSynonyms { get; set; }
        public List<List<string>> InfoLinkData { get; set; }
    }
}