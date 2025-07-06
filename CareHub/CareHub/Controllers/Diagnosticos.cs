using System.Collections; // Importa o namespace System.Collections, que fornece interfaces e classes que definem coleções de objetos.
using System.Globalization;
using System.Text; // Importa o namespace System.Text, necessário para classes como NormalizationForm .
using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.

// Importa o namespace System.Globalization, necessário para classes como CharUnicodeInfo .


namespace CareHub.Controllers; // Declara o namespace para o controller.

public class Diagnosticos : Controller // Declara a classe Diagnosticos
{
   
    // GET
    // Este método é uma action method que responde a requisições HTTP GET.
    // O parâmetro 'termo' é opcional e será usado para filtrar os diagnósticos.
    public IActionResult diagnosticos(string termo)
    {
        // Lê doencas.json localizado na pasta "wwwroot".
        // Este arquivo tem os dados dos diagnósticos em formato JSON.
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
        
        // Desserializa o JSON para uma lista de objetos InfoDiagnostico.
        // O JsonSerializer converte a string JSON numa coleção de objetos.
        var doencas = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);

        // Verifica se o 'termo' não é nulo ou vazio.
        if (!string.IsNullOrEmpty(termo))
        {
            // Converte o termo de pesquisa para minúsculas para uma comparação case-insensitive.
            termo = termo.ToLower();
            
            // Filtra a lista de doenças.
            // Seleciona as doenças cujo 'Nome' ou 'Categoria' (ambos convertidos para minúsculas)
            // contêm o 'termo' de pesquisa.
            doencas = doencas
                .Where(d => d.Nome.ToLower().Contains(termo) || d.Categoria.ToLower().Contains(termo))
                .ToList(); // Converte o resultado de volta para uma lista.
        }
        
        //remove qualquer doença onde a 'Categoria' é nula, vazia ou consiste apenas em espaços em branco.
        doencas = doencas
            .Where(d => !string.IsNullOrWhiteSpace(d.Categoria))   // remove categorias vazias ou nulas
            .ToList(); // Converte o resultado de volta para uma lista.
        

        // Armazena o termo de pesquisa na ViewBag.
        // A ViewBag permite passar dados do controller para a view.
        ViewBag.Termo = termo;
        
        // passamos a lista filtrada das doenças como modelo para a view.
        return View(doencas);
    }
    
    // Declaração de uma classe interna para representar a estrutura de um diagnóstico individual.
    public class InfoDiagnostico
    {
        // Atributo JsonPropertyName especifica que a propriedade 'Nome'
        [JsonPropertyName("nome")] 
        public string Nome { get; set; } // Propriedade para armazenar o nome do diagnóstico.

        // Atributo JsonPropertyName especifica que a propriedade 'Categoria'
        [JsonPropertyName("categoria")] 
        public string Categoria { get; set; } // Propriedade para armazenar a categoria do diagnóstico.

      
    }
}