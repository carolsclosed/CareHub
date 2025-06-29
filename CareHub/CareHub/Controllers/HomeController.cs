using System.Diagnostics; // Importa o namespace System.Diagnostics, que fornece classes para interagir com processos do sistema, registos de eventos e contadores de desempenho. Usado aqui para Activity.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using CareHub.Models; // Importa o namespace CareHub.Models
using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text;                // Importa o namespace System.Text, necessário para a classe StringBuilder e NormalizationForm.
using System.Globalization;       // Importa o namespace System.Globalization, necessário para CharUnicodeInfo e UnicodeCategory.
using static CareHub.Controllers.Diagnosticos; // Importa estaticamente a classe Diagnosticos do namespace CareHub.Controllers, permitindo o uso direto de membros estáticos (como InfoDiagnostico) sem qualificação completa.

namespace CareHub.Controllers; // Declara o namespace para o controller.

public class HomeController : Controller // Declara a classe HomeController
{
    private readonly ILogger<HomeController> _logger; // Declara uma variavel para o logger.

    // Construtor da classe HomeController.
    // Recebe uma instância de ILogger.
    // O logger é usado para registar informações, avisos e erros da aplicação.
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Action method para a página inicial (Index).
    // Responde a requisições HTTP GET.
    public IActionResult Index()
    {
        // Constrói o caminho completo para o arquivo 'doencas.json'.
        // Combina o diretório atual da aplicação (onde o executável está), "wwwroot" e o nome do arquivo.
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "doencas.json");
        // lê o json
        var jsonContent = System.IO.File.ReadAllText(filePath);
        // Desserializa o conteúdo JSON para uma lista de objetos InfoDiagnostico.
        var doencas = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);

        // Processa a lista de doenças:
        doencas = doencas
            .Where(d => !string.IsNullOrWhiteSpace(d.Categoria))   // 1. Remove entradas onde a categoria é nula, vazia ou consiste apenas em espaços em branco.
            .GroupBy(d => NormalizarTexto(d.Categoria)) // 2. Agrupa as doenças por uma versão da sua categoria.Isso garante que categorias como "Gripe" e "gripe" sejam tratadas como a mesma categoria.
            .Select(g => g.First())                     // 3. Para cada grupo (categoria), seleciona apenas a primeira.
            .OrderBy(d => d.Categoria)                  // 4. Ordena as doenças resultantes pelo nome original da categoria.
            .ToList();                                  // 5. Converte o resultado para uma lista.

        // Método estático auxiliar para normalizar strings (usado para as categorias).
        static string NormalizarTexto(string texto)
        {
            // Se o texto for null, vazio ou whitespace, retorna uma string vazia.
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            // 1. Remove espaços em branco no início e no fim da string.
            texto = texto.Trim();

            // 2. Converte a string para minúsculas usando a cultura invariante (consistente em todas as culturas).
            texto = texto.ToLowerInvariant();

            // 3. Remove acentos.
            // NormalizationForm.FormD decompõe caracteres nas suas partes base e diacríticos.
            texto = texto.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(); // Usa StringBuilder para construção eficiente de strings.

            foreach (var ch in texto)
            {
                // Verifica se o caractere não é uma marca de espaçamento .
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(ch); // Adiciona o caractere ao StringBuilder se não for um diacrítico.
                }
            }

            // NormalizationForm.FormC recompõe os caracteres combinados de volta à sua forma pré-composta, se possível.
            // Isso limpa a string final.
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

      // passamos a lista  de 'doencas' como modelo.
        return View(doencas);
    }

    // Action method para a página de privacidade.
    // Responde a requisições HTTP GET.
    public IActionResult Privacy()
    {
        return View(); 
    }

    // Atributo ResponseCache controla o armazenamento em cache da resposta HTTP.
    // Duration = 0: Não armazena em cache.
    // Location = ResponseCacheLocation.None: Não armazena em cache em nenhum sitio (cliente, proxy, servidor).
    // NoStore = true: Indica que o cliente (e proxies) não devem armazenar a resposta.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // Action method para lidar com erros.
    public IActionResult Error()
    {
        // Cria um novo objeto ErrorViewModel.
        // RequestId é obtido do Activity.Current ou do HttpContext.TraceIdentifier.
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}