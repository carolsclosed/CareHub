using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CareHub.Models;
using System.Text.Json;
using System.Text;                // <- necessário para NormalizationForm
using System.Globalization;       // <- necessário para CharUnicodeInfo
using static CareHub.Controllers.Diagnosticos;

namespace CareHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}