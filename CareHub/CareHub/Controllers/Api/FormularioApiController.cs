using System.Security.Claims;
using CareHub.Data;
using CareHub.Models.ApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class FormularioApiController :ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public FormularioApiController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: api/formularios
    [HttpGet]
    public ActionResult GetFormularios()
    {

        if (_context.Formularios == null)
        {
            return NotFound();
        }
        var formulario = _context.Formularios.Select(f => new FormularioApi{IdForm = f.IdForm, IdUtilizador = f.IdUtil,Nome = f.nome, Email = f.email, Telefone = f.telefone, Regiao = f.regiao, presencial = f.presencial, Descricao =f.descricao} ).ToList();
        
        var subClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (subClaim != null && subClaim.Value.Contains("admin"))
        {
            return Ok(formulario);
        }
        return Forbid(); 
        
    }
    
    //GET: api/Formulario
    [HttpGet("{id}")]
 
    public ActionResult GetFormulario(int id)
    {
        var formulario = _context.Formularios.Find(id);
        if (formulario == null)
        {
            return NotFound();
        }
        
        return Ok(formulario);
    }
}