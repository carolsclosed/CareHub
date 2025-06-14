using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace CareHub.Controllers
{
    [Authorize(Roles = "Administrador")]
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

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            // guardamos em sessão o id do utilizador que o utilizador quer editar
            // se ele fizer um post para um Id diferente, ele está a tentar alterar um utilizador diferente do que visualiza no ecrã
            HttpContext.Session.SetInt32("utilizadorId", utilizador.IdUtil);
            
            return View(utilizador);
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
            HttpContext.Session.SetInt32("utilizadorId", utilizadores.IdUtil);

            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.Include(p => p.ListaPosts).FirstAsync(u => u.IdUtil == id);
            if (utilizador != null)
            {
                // vou buscar o id do utilizador da sessão
                var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                // se o id do utilizador da sessão for diferente do que recebemos
                // quer dizer que está a tentar apagar um utilizador diferente do que tem no ecrã
                if (utilizadorDaSessao != id)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                
                // remover as fotos que ficam guardadas
                foreach (var item in utilizador.ListaPosts)
                {
                    
                    string localImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + item.Foto);
                    if (System.IO.File.Exists(localImagem))
                    {
                        System.IO.File.Delete(localImagem);
                    }
                }
                string imagemUtilizador = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+utilizador.Foto);
                string nomeFoto = System.IO.Path.GetFileName(imagemUtilizador);
                if (!nomeFoto.Equals("user.jpg", StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.Delete(imagemUtilizador);
                }
               
                
                IdentityUser userIdentity = await _userManager.FindByEmailAsync(utilizador.IdentityUserName);
                _userManager.DeleteAsync(userIdentity);
                
                _context.Utilizadores.Remove(utilizador);
            }

            
             
            
            await _context.SaveChangesAsync();
            // impede que tente fazer o apagar do mesmo utilizador
            HttpContext.Session.SetInt32("utilizadorId",0);
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.IdUtil == id);
        }
    }
}
