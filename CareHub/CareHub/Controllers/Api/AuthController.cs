using CareHub.Models.ApiModels;
using CareHub.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers.Api;

public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TokenService _tokenService;


    public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        TokenService tokenService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// método para fazer login
    /// se correto recebe o token JWT
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    //GET: api/auth/login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authenticar(LoginApiModel loginRequest)
    {
        // autenticação jwt
        var identityuser = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (identityuser == null)
        {
            return BadRequest("Palavra-passe ou utilizador errado");
        }
        
        var palavraPasse = await _signInManager.CheckPasswordSignInAsync(identityuser, loginRequest.Password, false);

        if (!palavraPasse.Succeeded)
        {
            return BadRequest("Palavra-passe ou utilizador errado");
        }
        
        var token = await _tokenService.GenerateToken(identityuser);
        
        return Ok (token);

        
        
        /* autenticação por sessão -> Identity

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok(loginRequest.Email);
            }
            else
            {
                return BadRequest("Erro no Login");
            }
            */
                  
    }
    
    //POST: api/auth/TerminarSessao
    [HttpPost]
    [Route("TerminarSessao")]
    public async Task<IActionResult> TerminarSessao()
    {
        await _signInManager.SignOutAsync();
        return Ok("");
    }
    
}