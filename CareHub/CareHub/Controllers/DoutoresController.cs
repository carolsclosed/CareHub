using CareHub.Data;
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.EntityFrameworkCore; 


namespace CareHub.Controllers;
/// <summary>
/// controller para os doutores
/// </summary>
public class DoutoresController : Controller 
{
    private readonly ApplicationDbContext _context; 
    
    
    public DoutoresController(ApplicationDbContext context)
    {
        _context = context; 
    }

    /// <summary>
    /// método para mostrar a lista de doutores
    /// </summary>
    /// <returns></returns>
    // GET
    public IActionResult Doutores()       
    {
      
      
        var doutores = _context.Doutores
            .Include(d => d.Utilizador) 
            .ToList(); 

        
        return View(doutores);
          
    }
}