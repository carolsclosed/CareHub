
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;

namespace CareHub.Services.WebSockts;

public class UpHub : Hub
{
    private readonly ILogger<UpHub> _logger;
    private readonly ApplicationDbContext _context;
    
    public UpHub(ILogger<UpHub> logger,ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User connected");
    }


    [Authorize]
    public void AtualizarUpvotes(int idPublicacao)
    {
        // ver se a publicação existe
        var publicacao = _context.Posts.Find(idPublicacao);
        if (publicacao == null)
            return;
        
        //utilizador
        var utilizador = _context.Utilizadores.Include(u => u.ListaUp).First(u => u.IdentityUserName == Context.User.Identity.Name);
        if (utilizador == null)
        {
            Clients.Caller.SendAsync("IniciarSessao");
            return;

        }
        
        //verificar se a relação já existe
        var jaUp = utilizador.ListaUp.FirstOrDefault(u => u.IdPost == idPublicacao);
        var Islike = false;
        
        if (jaUp != null)
        {
            publicacao.ListaUp.Remove(jaUp);
        }
        else
        {
           publicacao.ListaUp.Add(new Up()
           {
               IdPost = idPublicacao,
               IdUtil = utilizador.IdUtil
           });
           Islike = true;
        }

        _context.SaveChanges();
        
        Clients.All.SendAsync("AtualizarUpvotes", idPublicacao, _context.Ups.Where(up => up.IdPost == idPublicacao).Count() );
        Clients.Caller.SendAsync("AtualizarUpvotesPersonal", idPublicacao, Islike);
    }
    
    [Authorize]
    public async Task Comentar(Comentarios comentario)
    {
        //utilizador
        var utilizador = _context.Utilizadores.First(u => u.IdentityUserName == Context.User.Identity.Name);
        if (utilizador == null)
        { 
            await Clients.Caller.SendAsync("Erro", "Utilizador inválido");
            return;
        }
    
        comentario.IdUtil = utilizador.IdUtil;
        comentario.DataCom = DateOnly.FromDateTime(DateTime.Today);
    
        _context.Comentarios.Add(comentario); 
        await _context.SaveChangesAsync();
        
        var jsonCom = JsonConvert.SerializeObject(comentario, new JsonSerializerSettings(){ ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
    
        await Clients.Caller.SendAsync("ComentarAtualizar", 
            jsonCom, 
            utilizador.Foto,
            utilizador.Nome, 
            comentario.DataCom.ToString("dd-MM-yyyy"));
    }

    [Authorize]
    public void ApagarComentario(int idComentario, int idUtil)
    {
        var utilizador = _context.Utilizadores.First(u => u.IdentityUserName == Context.User.Identity.Name);
        if (utilizador == null || utilizador.IdUtil != idUtil)
        {
            Clients.Caller.SendAsync("Erro", "Utilizador inválido");
            return;
        }
        var comentario = _context.Comentarios.Find(idComentario);

        if (comentario == null)
        {
            
            return;
        }
        
        _context.Comentarios.Remove(comentario);
        _context.SaveChanges();
        
        
    }
    
}