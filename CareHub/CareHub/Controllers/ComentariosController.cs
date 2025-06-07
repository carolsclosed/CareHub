using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.EntityFrameworkCore;


namespace CareHub.Controllers;

public class ComentariosController : Controller
{
    private readonly ApplicationDbContext _context;
    
    
    public ComentariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Comentar([FromForm] Comentarios comentario)
    {
        var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);
        
        if (!ModelState.IsValid || utilizador == null)
        {
            return RedirectToAction("Index", "Publicacoes");
        }
        
        comentario.Utilizador = utilizador;
        
        comentario.DataCom= DateOnly.FromDateTime(DateTime.Today);
        
        comentario.IdUtil = utilizador.IdUtil;
        
        
        
        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Publicacoes");
        
    }
}

