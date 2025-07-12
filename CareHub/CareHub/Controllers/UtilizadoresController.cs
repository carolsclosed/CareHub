using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Controllers
{
    /// <summary>
    /// Controller para os utilizadores, apenas o admin tem acesso
    /// </summary>
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

        /// <summary>
        /// Método para obter a lista de utilizadores
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        /// <summary>
        /// Método para ver os detalhes de algum utilizador em específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .Include(u => u.ListaPosts)
                .FirstOrDefaultAsync(m => m.IdUtil == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        /// <summary>
        /// Método para obter a página de edição de um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
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

            HttpContext.Session.SetInt32("utilizadorId", utilizador.IdUtil);

            return View(utilizador);
        }

        /// <summary>
        /// Método para editar um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <param name="utilizadores"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar([FromRoute] int id, [Bind("IdUtil,Nome,NIF,Telemovel,Morada,CodPostal,Pais")] Utilizadores utilizadores)
        {
            if (id != utilizadores.IdUtil)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                    if (utilizadorDaSessao != id)
                    {
                        ModelState.AddModelError("", "Tentaste aldrabar isto palhaço! Outra vez!!!");
                        return View(utilizadores);
                    }

                    _context.Update(utilizadores);
                    await _context.SaveChangesAsync();
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

            HttpContext.Session.SetInt32("utilizadorId", utilizadores.IdUtil);

            return View(utilizadores);
        }

        /// <summary>
        /// Método para apagar um utilizador
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.Include(p => p.ListaPosts).FirstAsync(u => u.IdUtil == id);

            if (utilizador != null)
            {
                var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                if (utilizadorDaSessao != id || id == 1)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var item in utilizador.ListaPosts)
                {
                    string localImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + item.Foto);
                    if (System.IO.File.Exists(localImagem))
                    {
                        System.IO.File.Delete(localImagem);
                    }
                }

                string imagemUtilizador = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + utilizador.Foto);
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
            HttpContext.Session.SetInt32("utilizadorId", 0);
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.IdUtil == id);
        }
    }
}