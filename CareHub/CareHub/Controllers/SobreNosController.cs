using Microsoft.AspNetCore.Mvc;
using CareHub.Data;

namespace CareHub.Controllers;

public class SobreNosController :  Controller
{
    public IActionResult Index()
    {
        return View("SobreNos");
    }


}