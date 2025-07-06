using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CareHub.Data;
using CareHub.Models.ApiModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class UtilizadoresApiController :ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UtilizadoresApiController(ApplicationDbContext context)
    {
        _context = context ;
    }

    //GET: api/Utilizadores
    [HttpGet]
    public ActionResult GetUtilizadores()
    {
        var utilzador = _context.Utilizadores
            .Select(u => new UtilizadoresApi { Id = u.IdUtil, Nome = u.Nome, Regiao = u.Regiao }).ToList();

        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (roleClaim != null && roleClaim.Value == "admin")
        {
            return Ok(utilzador);    
        }

        
        return BadRequest("Não tem acesso");    
        
    }
    
}