using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers;

public class Doutores : Controller
{
    // GET
    public IActionResult doutores()
    {
        return View();
    }
}