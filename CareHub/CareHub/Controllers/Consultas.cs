using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers;

public class Consultas : Controller
{
    // GET
    public IActionResult consultas()
    {
        return View();
    }
}