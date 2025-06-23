using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers;

public class DoutoresController : Controller
{
    // GET
    public IActionResult doutores()
    {
        return View();
    }
}