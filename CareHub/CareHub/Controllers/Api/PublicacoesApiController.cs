using System.Security.Claims;
using CareHub.Data;
using CareHub.Models;
using CareHub.Models.ApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;


/// <summary>
/// controller das publicações para a API
/// </summary>
[Route("api/publicacoes")]
[ApiController]
public class PublicacoesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PublicacoesApiController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Método para ver todas as publicações
    /// </summary>
    /// <returns></returns>
    //GET: api/publicacoes
    [HttpGet]
    public ActionResult GetPublicacoes()
    {
        if (User.Identity.IsAuthenticated)
        {
            var resultadoAut = _context.Posts.ToList();
            return Ok(resultadoAut);
        }
        
        var resultado = _context.Posts.Select(p=> new PublicacoesApi{Id = p.IdPost,Titulo = p.TituloPost, Categoria = p.Categoria, Texto = p.TextoPost, DataPub = p.DataPost, Foto = p.Foto}).ToList();

        return Ok(resultado);
    }
    
    /// <summary>
    /// Método para ver uma publicação em específico
    /// verifica se o utilizador que quer ver é o admin ou o dono da publicação
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //GET: api/publicacoes/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetPublicacoes(int id)
    {
        var publicacao = await _context.Posts.FindAsync(id);
        if (publicacao == null)
        {
            return NotFound("Publicação não encontrada!");
        }
        var utilizadorPublicacao = await _context.Utilizadores.FindAsync(publicacao.IdUtil);
        var resultado = _context.Posts.Where(p => p.IdPost == id).Select(p => new PublicacoesApi
        {
            Id = p.IdPost, Titulo = p.TituloPost, Categoria = p.Categoria, Texto = p.TextoPost, DataPub = p.DataPost,
            Foto = p.Foto
        });
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (utilizadorPublicacao.IdentityUserName != User.Identity.Name && email != "admin@mail.pt")
        {
            return Unauthorized("Apenas o utilizador que publicou e o admin têm acesso");
        }
        
        return Ok(resultado);
    }

    /// <summary>
    /// Método para criar uma publicação sem imagem
    /// É preciso estar logado para poder publicar 
    /// </summary>
    /// <param name="Titulo"></param>
    /// <param name="Categoria"></param>
    /// <param name="Texto"></param>
    /// <returns></returns>
    [HttpPost("criar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Criar(string Titulo,string Categoria, String Texto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var utilizador = _context.Utilizadores.First(u => u.IdentityUserName == email);

        if (utilizador == null)
        {
            return BadRequest("Tem que estar logado para publicar!");
        }
        
        var publicacao = new Posts
        {
            
            IdUtil = utilizador.IdUtil,
            TituloPost = Titulo,
            Categoria = Categoria,
            TextoPost = Texto,
            DataPost = DateOnly.FromDateTime(DateTime.Today),
            Utilizador = utilizador
           
        };
            
        _context.Add(publicacao); 
        await _context.SaveChangesAsync();
        
        return Ok("A publicação foi publicada com sucesso!");
        
    }

    /// <summary>
    /// Método para editar uma publicação
    /// Se a publicação já tiver imagem, continuará com ela
    /// Não é possível editar a imagem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Titulo"></param>
    /// <param name="Categoria"></param>
    /// <param name="Texto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Editar(int id, string Titulo, string Categoria, String Texto)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == email);
        
        if (utilizador == null)
        {
            return BadRequest("Tem que estar logado para editar!");
        }
        
        var publicacao = await _context.Posts.FindAsync(id);

        if (publicacao == null)
        {
            return NotFound("Publicação não encontrada!");
        }
        
        if (utilizador.IdUtil != publicacao.IdUtil && utilizador.IdentityRole != "Administrator")
        {
            return Unauthorized("Tem que ser o dono da publicação ou o admin para poder editar");
        }
        
        publicacao.TituloPost = Titulo;
        publicacao.Categoria = Categoria;
        publicacao.TextoPost = Texto;
        publicacao.DataPost = DateOnly.FromDateTime(DateTime.Today);
        
        _context.Update(publicacao);
        await _context.SaveChangesAsync();
        
        return Ok("Publicação editada com sucesso!");

    }

    /// <summary>
    /// Método para apagar uma publicação
    /// Apenas o dono da publicação ou o admin podem apagar
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("apagar/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Apagar(int id)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == email);
        
        if (utilizador == null)
        {
            return BadRequest("Tem que estar logado para apagar!");
        }
        
        var publicacao = await _context.Posts.FindAsync(id);

        if (publicacao == null)
        {
            return NotFound("Publicação não encontrada!");
        }

        
        if (utilizador.IdUtil != publicacao.IdUtil && utilizador.IdentityRole != "Administrator")
        {
            return Unauthorized("Tem que ser o dono da publicação ou o admin para poder apagar");
        }
        
        _context.Remove(publicacao);
        await _context.SaveChangesAsync();
        return Ok("Publicação apagada com sucesso!");
    }
    
}