using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.

namespace CareHub.Controllers; // Declara o namespace para o controller.

public class SobreNosController : Controller // Declara a classe SobreNosController
{
    // "Index" para a página "Sobre Nós".
    // Este método é uma action method que responde a requisições HTTP GET.
    public IActionResult Index()
    {
        return View("SobreNos");
    }
}