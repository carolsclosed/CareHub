using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models_Comentarios = CareHub.Models.Comentarios;

namespace CareHub.Services.WebSockts;

public class ComentarioHub : Hub
{
    private readonly ApplicationDbContext _context;

    
    public ComentarioHub(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    [HttpPost]
    public async Task Comentar(Comentarios comentario)
    {
        
        //utilizador
        var utilizador = _context.Utilizadores.First(u => u.IdentityUserName == Context.User.Identity.Name);
        if (utilizador == null)
        {
            Clients.Caller.SendAsync("Erro","Utilizador inválido");
            return;
        }
        
        comentario.Utilizador = utilizador;
        comentario.DataCom = DateOnly.FromDateTime(DateTime.Today);
        comentario.IdUtil = utilizador.IdUtil;

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        
        
    }
    
    
}