using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CareHub.Controllers
{
    [Authorize(Roles = "admin")]
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UtilizadoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        // GET: Utilizadores/Detalhes/5
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .Include(u=>u.ListaPosts)
                .FirstOrDefaultAsync(m => m.IdUtil == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }
        

        // GET: Utilizadores/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }
            // guardamos em sessão o id do utilizador que o utilizador quer editar
            // se ele fizer um post para um Id diferente, ele está a tentar alterar um utilizador diferente do que visualiza no ecrã
            HttpContext.Session.SetInt32("utilizadorId", utilizadores.IdUtil);
            
            return View(utilizadores);
        }

        // POST: Utilizadores/Editar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromRoute]int id, [Bind("Id,Nome,NIF,Telemovel,Morada,CodPostal,Pais")] Utilizadores utilizadores)
        {
            
            if (id != utilizadores.IdUtil  )
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // vou buscar o id do utilizador da sessão
                    var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                    // se o id do utilizador da sessão for diferente do que recebemos
                    // quer dizer que está a tentar alterar um utilizador diferente do que tem no ecrã
                    if (utilizadorDaSessao != id)
                    {
                        ModelState.AddModelError("", "Tentaste aldrabar isto palhaço! Outra vez!!!");
                        return View(utilizadores);
                    }
                    
                    
                    _context.Update(utilizadores);
                    await _context.SaveChangesAsync();
                    // colocamos o utilizadorId da sessão a 0, para ele não poder fazer POSTs sucessivos 
                    HttpContext.Session.SetInt32("utilizadorId", 0);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizadores.IdUtil))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilizadores);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            
            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.IdUtil == id);
            if (utilizadores == null)
            {
                return NotFound();
            }
            
            // guardamos em sessão o id do utilizador que o utilizador quer apagar
            // se ele fizer um post para um Id diferente, ele está a tentar apagar um utilizador diferente do que visualiza no ecrã
            HttpContext.Session.SetInt32("IdUtil", utilizadores.IdUtil);

            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null) 
            {
                // Caso o utilizador não exista, redireciona para o índice
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Administrador");
            var identityUser = await _userManager.FindByNameAsync(utilizador.IdentityUserName);
            var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId"); // Buscar o id do utilizador na sessão

            // Administradores podem apagar qualquer utilizador
            if (isAdmin)
            {
                _context.Utilizadores.Remove(utilizador);
                // _context.Posts.RemoveRange(utilizador.ListaPosts);
                if (identityUser != null) 
                {
                    await _userManager.DeleteAsync(identityUser);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // O utilizador só pode apagar o seu próprio registo
            if (utilizadorDaSessao == id)
            {
                _context.Utilizadores.Remove(utilizador);
                if (identityUser != null)
                {
                    await _userManager.DeleteAsync(identityUser);
                }
                await _context.SaveChangesAsync();
                // Limpamos o id da sessão após a remoção
                HttpContext.Session.SetInt32("utilizadorId", 0);
                return RedirectToAction(nameof(Index));
            }

            // Se o utilizador tentou apagar outro id, redireciona para o índice
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.IdUtil == id);
        }
    }
}
