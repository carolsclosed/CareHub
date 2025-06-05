using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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
            return;
        }
        
        
        if (utilizador == null)
        {
            return;
        }
        //verificar se a relação já existe
        var jaUp = utilizador.ListaUp.FirstOrDefault(u => u.IdPost == idPublicacao);
        
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
        }

        _context.SaveChanges();
        
        Clients.All.SendAsync("AtualizarUpvotes", idPublicacao, _context.Ups.Where(up => up.IdPost == idPublicacao).Count() );
        
    }
    
}