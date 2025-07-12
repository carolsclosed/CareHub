using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CareHub.Data;
using CareHub.Models;
using CareHub.Models.ApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

/// <summary>
/// Controller dos utilizadores para a API, apenas o admin tem acesso
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/utilizadores")]
[ApiController]
public class UtilizadoresApiController :ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public UtilizadoresApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context ;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Método para ver todos os utilizadores
    /// </summary>
    /// <returns></returns>
    //GET: api/Utilizadores
    [HttpGet]
    public ActionResult GetUtilizadores()
    {
        var utilzador = _context.Utilizadores
            .Select(u => new UtilizadoresApi { Id = u.IdUtil, Nome = u.Nome, Regiao = u.Regiao }).ToList();
        

        if (!IsAdmin()){
            return Unauthorized("Apenas o admin tem acesso!");
        }

        
        return Ok(utilzador);    
        
    }
    
    /// <summary>
    /// Método para ver um utilizador em específico
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult GetUtilizador(int id)
    {

        if (!IsAdmin())
        {
            return Unauthorized("Apenas o admin pode ver algum utilizador");
        }
        var utilzador = _context.Utilizadores.Where(u => u.IdUtil == id).ToList();



        return Ok(utilzador);
    }
    
    /// <summary>
    /// Método para criar um utilizador
    /// </summary>
    /// <param name="Nome"></param>
    /// <param name="Regiao"></param>
    /// <param name="Telefone"></param>
    /// <param name="email"></param>
    /// <param name="PalavraPasse"></param>
    /// <param name="ConfirmarPalavraPasse"></param>
    /// <returns></returns>
    [HttpPost("criar")]
    public async Task<IActionResult> CriarUtilizador(string Nome, string Regiao, string Telefone, string email, string PalavraPasse, String ConfirmarPalavraPasse)
    {

        if (!IsAdmin())
        {
            return Unauthorized("Só o admin pode criar utilizadores!");
        }

        if (email == null || email == "")
        {
            return BadRequest("O email não pode ser vazio");
        }

        if (PalavraPasse != ConfirmarPalavraPasse)
        {
            return BadRequest("As palavras passe não coincidem");
        }
        
        var identityUser = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(identityUser, PalavraPasse);
        if (!result.Succeeded)
        {
            
            var error = result.Errors.FirstOrDefault()?.Description ?? "Erro ao criar utilizador.";
            return BadRequest(error);
        }
        
        var utilizador = new Utilizadores
        {
            Nome = Nome,
            Regiao = Regiao,
            Telefone = Telefone,
            IdentityUserName = email,
            IdentityRole = "Utilizador",
            Foto = "/ImagensUtilizadores/user.jpg"
        };

       
        _context.Add(utilizador);
        await _context.SaveChangesAsync();
        
        return Ok("Utilizador criado com sucesso!");

    }

    /// <summary>
    /// método para editar um utilizador
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Nome"></param>
    /// <param name="Regiao"></param>
    /// <param name="Telefone"></param>
    /// <returns></returns>
    [HttpPut("editar/{id}")]
    public ActionResult editar(int id, string Nome, string Regiao, string Telefone)
    {

        if (!IsAdmin())
        {
            return Unauthorized("Apenas o admin pode editar utilizadores");
        }
        
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdUtil == id);
        
        utilizador.Nome = Nome;
        utilizador.Regiao = Regiao;
        utilizador.Telefone = Telefone;
        
        _context.Update(utilizador);
        _context.SaveChanges();
        
        return Ok("Utilizador editado com sucesso!");
    }
    
    /// <summary>
    /// Método para apagar um utilizador
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("/apagar/{id}")]
    public async Task<ActionResult> ApagarUtilizador(int id)
    {
        
        if (!IsAdmin())
        {
            return Unauthorized("Apenas o admin pode apagar utilizadores");
        }
        
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdUtil == id);

        if (utilizador == null)
        {
            return BadRequest("O utilizador com esse id não existe!");
        }
        
        foreach (var item in utilizador.ListaPosts)
        {
            string localImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + item.Foto);
            if (System.IO.File.Exists(localImagem)) // Verifica se o arquivo existe antes de tentar apagar.
            {
                System.IO.File.Delete(localImagem); // Apaga o arquivo da imagem.
            }
        }
        
        string imagemUtilizador = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+utilizador.Foto);
        string nomeFoto = System.IO.Path.GetFileName(imagemUtilizador);
        if (!nomeFoto.Equals("user.jpg", StringComparison.OrdinalIgnoreCase)) // Compara ignorando maiúsculas/minúsculas.
        {
            if (System.IO.File.Exists(imagemUtilizador))
            {
                try
                {
                    System.IO.File.Delete(imagemUtilizador);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Logar erro e seguir
                    Console.WriteLine($"[ERRO] Sem permissão para apagar imagem: {ex.Message}");
                }
            }
        }
        
        // Obtém o objeto IdentityUser associado ao utilizador da aplicação.
        IdentityUser userIdentity = await _userManager.FindByEmailAsync(utilizador.IdentityUserName);

        if (userIdentity != null)
        {
            // Apaga o utilizador do ASP.NET Core Identity.
            _userManager.DeleteAsync(userIdentity);
        }
        
                

        
        _context.Utilizadores.Remove(utilizador);
        await _context.SaveChangesAsync();
        return Ok("Utilizador apagado com sucesso!");
    }
    
    
    /// <summary>
    /// Método para verificar se o utilizador é o admin
    /// </summary>
    /// <returns></returns>
    private bool IsAdmin()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email != "admin@mail.pt")
        {
            return false;
        }
        
        return true;
    }
}