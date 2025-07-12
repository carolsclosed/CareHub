using System.Security.Claims;
using CareHub.Data;
using CareHub.Models;
using CareHub.Models.ApiModels;
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

/// <summary>
/// Controller dos formulários para a API
/// </summary>

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/formularios")]
[ApiController]
public class FormularioApiController :ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMailer _mailer;
    public FormularioApiController(ApplicationDbContext context, IMailer mailer)
    {
        _context = context;
        _mailer = mailer;
    }
    
    /// <summary>
    /// Método para obter os formulários
    /// apenas o admin tem acesso
    /// </summary>
    /// <returns></returns>
    // GET: api/formularios
    [HttpGet]
    public ActionResult GetFormularios()
    {

        if (_context.Formularios == null)
        {
            return NotFound();
        }
        var formulario = _context.Formularios.Select(f => new FormularioApi{IdForm = f.IdForm, IdUtilizador = f.IdUtil,Nome = f.nome, Email = f.email, Telefone = f.telefone, Regiao = f.regiao, presencial = f.presencial, Descricao =f.descricao} ).ToList();
        

        if (!IsAdmin() || User.Identities.First().IsAuthenticated == false )
        {
            return BadRequest("Apenas o admin pode ver todos os formulários");
        }
        
        return Ok(formulario);
        
    }
    
    /// <summary>
    /// Método para obter um formolário em específico
    /// Apenas o admin tem acesso
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //GET: api/formularios{id}
    [HttpGet("{id}")]
    public ActionResult GetFormulario(int id)
    {
        var formulario = _context.Formularios.Where(f => f.IdForm == id).Select(f => new FormularioApi{IdForm = f.IdForm, IdUtilizador = f.IdUtil,Nome = f.nome, Email = f.email, Telefone = f.telefone, Regiao = f.regiao, presencial = f.presencial, Descricao =f.descricao} ).ToList();
        
        if (!IsAdmin())
        {
            return Forbid();
        }
        return Ok(formulario);
    }

    /// <summary>
    /// Método para editar um formulário caso o utilizador detete
    /// que alguma informação está errada no email que recebeu automaticamente,
    /// e assim o admin alterar
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Nome"></param>
    /// <param name="Regiao"></param>
    /// <param name="Telefone"></param>
    /// <param name="presencial"></param>
    /// <returns></returns>
    [HttpPut("editar/{id}")]
    public ActionResult editar(int id, string Nome, string Regiao, int Telefone, bool presencial)
    {
        if (!IsAdmin())
        {
            return BadRequest("Apenas o admin tem acesso!");
        }

        var formulario = _context.Formularios.FirstOrDefault(f => f.IdForm == id);
        
        formulario.nome = Nome;
        formulario.regiao = Regiao;
        formulario.telefone = Telefone;
        formulario.presencial = presencial;
        
        _context.Update(formulario);
        _context.SaveChanges();

        return Ok("Formulario editado com sucesso!");
    }

    /// <summary>
    /// Método para criar formulário de consulta
    /// é enviado um email para quem fez o formulário
    /// </summary>
    /// <param name="Nome"></param>
    /// <param name="Email"></param>
    /// <param name="Telefone"></param>
    /// <param name="Regiao"></param>
    /// <param name="Descricao"></param>
    /// <param name="presencial"></param>
    /// <returns></returns>
    [HttpPost("criar")]
    public async Task<ActionResult> CriarFormulario(string Nome, string Email, int Telefone, string Regiao, string Descricao, bool presencial )
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == email);
        
        
        var form = new Formularios
        {
            IdUtil = utilizador.IdUtil,
            email = Email,
            descricao = Descricao,
            nome = Nome,
            presencial = presencial,
            regiao = Regiao,
            telefone = Telefone,
            Utilizador = utilizador
        };
        
        var emailCorpo = "";
        //preparar o email
        if (form.presencial == false)
        {
            emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Não <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
        }
        else
        {
            emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Sim <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
        }
            
        var emailAssunto = "Confirmação do formulário";

        try
        {
                
            //enviar email de confirmação
            await _mailer.SendEmailAsync(form.email, emailAssunto, emailCorpo);

            _context.Add(form); // Adiciona o objeto 'form' ao contexto para ser inserido na base de dados.
            await _context.SaveChangesAsync(); // guarda as mudanças na base de dados.

            return Ok("Formulario criado com sucesso!");
        }
        catch (Exception ex)
        {
           return BadRequest("Ocurreu um erro ao enviar o email, o seu formulário não foi submetido. Por favor, tente novamente mais tarde.");
        }

        
    }
    
    /// <summary>
    /// Método para apahar formulário, caso o admin assim o queira
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("apagar/{id}")]
    public ActionResult ApagarFormulario(int id)
    {
        if (!IsAdmin())
        {
            return BadRequest("Apenas o admin tem acesso!");
        }

        var formulario = _context.Formularios.FirstOrDefault(f => f.IdForm == id);

        if (formulario == null)
        {
            return BadRequest("Formulário com o id = (" + id +") não existe!");
        }
        
        _context.Formularios.Remove(formulario);
        _context.SaveChanges();
        
        return Ok("Formulario apagado com sucesso!");
        
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