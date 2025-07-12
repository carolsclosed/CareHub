using System.Text.Json;
using System.Text.Json.Serialization;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Controllers
{
    /// <summary>
    /// Controller para as publicações
    /// </summary>
    public class PublicacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublicacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método para obter a lista de publicações
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var listaPosts = _context.Posts
                .Include(p => p.ListaUp)
                .Include(p => p.Utilizador)
                .Include(p => p.ListaComentarios).ThenInclude(c => c.Utilizador);
            
            return View(await listaPosts.ToListAsync());
        }
        /// <summary>
        /// Método para a página de criação de publicações
        /// </summary>
        /// <param name="id"></param>
        /// <param name="termo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Criar(string? id, string? termo)
        {
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
            var categorias = JsonSerializer.Deserialize<List<InfoCategoria>>(jsonContent);

            if (!string.IsNullOrEmpty(termo))
            {
                termo = termo.ToLower();
                categorias = categorias
                    .Where(r => r.Categoria.ToLower().Contains(termo))
                    .ToList();
            }

            var categoriasDropdown = categorias
                .SelectMany(r => new[] { r.Categoria })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Categorias = categoriasDropdown ?? new List<string>();
            ViewBag.Termo = termo;

            Console.WriteLine("Current logged in user: " + User.Identity.Name);

            return View();
        }

        /// <summary>
        /// Método para criação de publicações com imagem
        /// </summary>
        /// <param name="post"></param>
        /// <param name="imagem"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Criar(
            [Bind("TituloPost,TextoPost,Categoria,DataPost")] Posts post,
            IFormFile imagem)
        {
            bool haImagem = false;
            var nomeImagem = "";

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);

            if (utilizador == null)
            {
                ModelState.AddModelError("", "Utilizador não encontrado.");
            }

            post.Utilizador = utilizador;
            post.IdUtil = utilizador.IdUtil;

            if (ModelState.IsValid)
            {
                post.DataPost = DateOnly.FromDateTime(DateTime.Now);

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
        /// <summary>
        /// Método para obter os detalhes de uma publicação
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (post == null) return NotFound();

            return View(post);
        }
        /// <summary>
        /// Métoddo para obter a página de edição de uma publicação 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Publicacao = _context.Posts
                .Include(f => f.Utilizador)
                .FirstOrDefault(f => f.IdPost == id);
            
            if (Publicacao == null)
            {
                return NotFound();
            }

            if (Publicacao.Utilizador.IdentityUserName != User.Identity.Name)
            {
                return RedirectToAction(nameof(Index)); 
            }
            
            return View(Publicacao);
        }

        /// <summary>
        /// Método para editar uma publicação com e sem imagem, é possível adicionar uma imagem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publicacao"></param>
        /// <param name="imagem"></param>
        /// <returns></returns>
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

            if (user == null || publicacaoExistente.IdUtil != user.IdUtil)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                if (haImagem)
                {
                    if (publicacaoExistente.Foto != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", publicacaoExistente.Foto);
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
        /// <summary>
        /// Método para obter a página de confirmação para apagar uma publicação
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
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
        /// <summary>
        /// Método para apagar uma publicação caso haja imagem ela é apagada da memória
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
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
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.Foto);
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