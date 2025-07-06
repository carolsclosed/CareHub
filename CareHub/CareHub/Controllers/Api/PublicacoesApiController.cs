using CareHub.Data;
using CareHub.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class PublicacoesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PublicacoesApiController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    //GET: api/publicacoes
    [HttpGet]
    public ActionResult GetPublicacoes()
    {
        if (User.Identity.IsAuthenticated)
        {
            var resultadoAut = _context.Posts.ToList();
            return Ok(resultadoAut);
        }
        
        var resultado = _context.Posts.Select(p=> new PublicacoesApi{Titulo = p.TituloPost, Categoria = p.Categoria, Texto = p.TextoPost, DataPub = p.DataPost, Foto = p.Foto}).ToList();

        return Ok(resultado);
    }
    
    //GET: api/publicacoes/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetPublicacoes(int id)
    {
        var publicacao = await _context.Posts.FindAsync(id);
        if (publicacao == null)
        {
            return NotFound();
        }
        
        return Ok(publicacao);
    }
    
}