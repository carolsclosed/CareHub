using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using CareHub.Models;
using Microsoft.AspNetCore.Mvc;
using static CareHub.Controllers.Diagnosticos;

namespace CareHub.Controllers;

/// <summary>
/// Controller da página inicial
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Método do Index
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "doencas.json");
        var jsonContent = System.IO.File.ReadAllText(filePath);
        var doencas = JsonSerializer.Deserialize<List<InfoDiagnostico>>(jsonContent);

        doencas = doencas
            .Where(d => !string.IsNullOrWhiteSpace(d.Categoria))
            .GroupBy(d => NormalizarTexto(d.Categoria))
            .Select(g => g.First())
            .OrderBy(d => d.Categoria)
            .ToList();

        static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            texto = texto.Trim();
            texto = texto.ToLowerInvariant();
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

    /// <summary>
    /// Método para a política de privacidade
    /// </summary>
    /// <returns></returns>
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