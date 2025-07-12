using Microsoft.AspNetCore.Mvc; 

namespace CareHub.Controllers; 

public class SobreNosController : Controller 
{
    /// <summary>
    /// Método para obter a página com a nossa informação
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        return View("SobreNos");
    }
}