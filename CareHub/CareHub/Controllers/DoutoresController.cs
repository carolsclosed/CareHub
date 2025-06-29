using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.EntityFrameworkCore; // Importa o namespace Microsoft.EntityFrameworkCore, que fornece classes e funcionalidades para trabalhar com o Entity Framework Core (ORM).
using CareHub.Data; // Importa o namespace CareHub.Data - applicationdbcontext

namespace CareHub.Controllers; // Declara o namespace para o controller. 

public class DoutoresController : Controller // Declara a classe DoutoresController
{
    private readonly ApplicationDbContext _context; // Declara uma variavel para a instância do capllicationdbcontexts.
    
    // Construtor da classe DoutoresController.
    // Recebe uma instância de ApplicationDbContext.
    // A instância é atribuída a variavel _context, permitindo que o controller interaja com a base de dados.
    public DoutoresController(ApplicationDbContext context)
    {
        _context = context; 
    }

    // GET
    //action method que responde a requisições HTTP GET para exibir a lista de doutores.
    public IActionResult Doutores()       // Obtém os doutores
    {
      // O método.Include(d => d.Utilizador) instrui o Entity Framework Core
      // a carregar os dados do objeto 'Utilizador' relacionado, pois cada doutor é um utilizador.
      // Sem o .Include(), a propriedade 'Utilizador' de cada doutor seria nula.
      
        var doutores = _context.Doutores
            .Include(d => d.Utilizador)  // Carrega os dados do utilizador associado a cada doutor.
            .ToList(); // Converte o resultado da consulta para uma lista

        // passamos a lista de doutores (agora com os dados dos utilizadores)
        // como modelo para a view.
        return View(doutores);
          
    }
}