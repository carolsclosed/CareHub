using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers;

public class Consultas : Controller
{
    // GET
    public IActionResult consultas()
    {
        return View();
    }

    public async Task<IActionResult> onlinePresencial()
    {
        return View();
    }
    
    public async Task<IActionResult> presencial()
    {
        return View();
    }
    
    public async Task<IActionResult> online()
    {
        return View();
    }
}