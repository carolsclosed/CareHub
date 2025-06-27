using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CareHub.Controllers
{
    public class PublicacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacoes
        public async Task<IActionResult> Index()
        {
            var listaPosts = _context.Posts
                .Include(p => p.ListaUp)
                .Include(p => p.Utilizador)
                .Include(p => p.ListaComentarios).ThenInclude(c => c.Utilizador);
            return View(await listaPosts.ToListAsync());
        }

        // GET: Publicacoes/Criar
        [Authorize]
        public IActionResult Criar(string? id, string? termo)
        {
       
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
            var categorias = JsonSerializer.Deserialize<List<InfoCategoria>>(jsonContent);

            if (!string.IsNullOrEmpty(termo))
            {
                termo = termo.ToLower();
                categorias = categorias
                    .Where(r =>
                        r.Categoria.ToLower().Contains(termo))
                    .ToList();
            }

            // Junta todas as strings únicas (nome, distrito, provincia)
            var categoriasDropdown = categorias
                .SelectMany(r => new[] { r.Categoria})
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Categorias = categoriasDropdown ?? new List<string>();
            ViewBag.Termo = termo;

            
            Console.WriteLine("Current logged in user: " + User.Identity.Name);

            return View();
        }

        // POST: Publicacoes/Criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Criar(
            [Bind("TituloPost,TextoPost,Categoria,DataPost")] Posts post,
            IFormFile imagem)
        {
            bool haImagem = false;
            var nomeImagem = "";

            // Obter o utilizador autenticado
            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);

            if (utilizador == null)
            {
                ModelState.AddModelError("", "Utilizador não encontrado.");
            }
            
            post.Utilizador = utilizador;

            if (ModelState.IsValid)
            {
                post.DataPost = DateOnly.FromDateTime(DateTime.Now);
                post.IdUtil = utilizador.IdUtil;
                

                if (imagem != null && 
                    (imagem.ContentType == "image/png" || imagem.ContentType == "image/jpeg"))
                {
                    haImagem = true;
                    Guid g = Guid.NewGuid();
                    nomeImagem = g + Path.GetExtension(imagem.FileName).ToLowerInvariant();
                    post.Foto = "imagens/" + nomeImagem;
                }

                if (haImagem)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    filePath = Path.Combine(filePath, nomeImagem);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recarregar categorias para o dropdown
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
            var categorias = JsonSerializer.Deserialize<List<InfoCategoria>>(jsonContent);
            var categoriasDropdown = categorias
                .SelectMany(r => new[] { r.Categoria })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Categorias = categoriasDropdown ?? new List<string>();

            
            return View(post);
            
        }
        
        
    

        // GET: Publicacoes/Detalhes/5
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (post == null) return NotFound();

            return View(post);
        }
        
        
        // GET: Fotografias/Editar/5
        [Authorize]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Publicacao =  _context.Posts
                .Include(f => f.Utilizador)
                .FirstOrDefault(f => f.IdPost == id);
            
            if (Publicacao == null)
            {
                return NotFound();
            }

            

            if (Publicacao.Utilizador.IdentityUserName != User.Identity.Name && User.Identity.Name != "admin@mail.pt")
            {
                return RedirectToAction(nameof(Index));
            }
            
      
            return View(Publicacao);
        }

        // POST: Fotografias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Editar(int id,
            [Bind("IdPost,TituloPost,Categoria,TextoPost")] Posts publicacao, IFormFile imagem)
        {
            var haImagem = false;
            var nomeImagem = "";
            
            if (id != publicacao.IdPost)
            {
                return NotFound();
            }

            if (imagem != null && 
                (imagem.ContentType == "image/png" || imagem.ContentType == "image/jpeg"))
            {
                haImagem = true;
                Guid g = Guid.NewGuid();
                nomeImagem = g + Path.GetExtension(imagem.FileName).ToLowerInvariant();
                publicacao.Foto = "imagens/" + nomeImagem;
            }
            
            
            if (haImagem)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath = Path.Combine(filePath, nomeImagem);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }
            }
            
            var publicacaoExistente = await _context.Posts
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (publicacaoExistente == null)
            {
                return NotFound();
            }

            var user = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);

            if (user == null && User.Identity.Name != "admin@mail.pt" || publicacaoExistente.IdUtil != user.IdUtil && User.Identity.Name != "admin@mail.pt")
            {
                return Forbid(); // User is not the owner
            }

            if (ModelState.IsValid)
            {
                if (haImagem)
                {
                    if (publicacaoExistente.Foto != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", publicacaoExistente.Foto);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    publicacaoExistente.Foto = publicacao.Foto;
                }
                
                
                publicacaoExistente.TituloPost = publicacao.TituloPost;
                publicacaoExistente.Categoria = publicacao.Categoria;
                publicacaoExistente.TextoPost = publicacao.TextoPost;
                publicacaoExistente.DataPost = DateOnly.FromDateTime(DateTime.Now);
                
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Posts.Any(e => e.IdPost == publicacao.IdPost))
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

            return View(publicacaoExistente);
        }
        

        // GET: Publicacoes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (post == null) return NotFound();

            return View(post);
        }

        // POST: Publicacoes/Delete/5
        // POST: Publicacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post != null)
            {
                if (post.Foto != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", post.Foto);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        
       
        public class InfoCategoria
        {
            [JsonPropertyName("nome")] 
            public string Nome { get; set; }
            [JsonPropertyName("categoria")] 
            public string Categoria { get; set; }
        }
    }
}
