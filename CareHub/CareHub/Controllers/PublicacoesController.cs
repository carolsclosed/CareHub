using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.EntityFrameworkCore; // Importa o namespace Microsoft.EntityFrameworkCore, que fornece classes e funcionalidades para trabalhar com o Entity Framework Core (ORM).
using CareHub.Data; // Importa o namespace CareHub.Data
using CareHub.Models; // Importa o namespace CareHub.Models
using Microsoft.AspNetCore.Authorization; // Importa o namespace Microsoft.AspNetCore.Authorization, usado para controlo de acesso e autorização.

namespace CareHub.Controllers // Declara o namespace para o controller.
{
    public class PublicacoesController : Controller // Declara a classe PublicacoesController
    {
        private readonly ApplicationDbContext _context; // Declara uma variavel para a instância applicationdbcontext.

        // Construtor da classe PublicacoesController.
        // Recebe uma instância de ApplicationDbContext
        // A instância é atribuída a variavel _context, permitindo a interação com a base de dados.
        public PublicacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Publicacoes
        // action method que responde a requisições HTTP GET para exibir a lista de publicações.
        public async Task<IActionResult> Index()
        {
            // Consulta os Posts na base de dados e inclui dados relacionados:
            // .Include(p => p.ListaUp): Carrega os "ups" (curtidas/reações) associados a cada post.
            // .Include(p => p.Utilizador): Carrega os dados do utilizador que criou o post.
            // .Include(p => p.ListaComentarios).ThenInclude(c => c.Utilizador): Carrega os comentários e, para cada comentário, o utilizador que o fez.
            var listaPosts = _context.Posts
                .Include(p => p.ListaUp)
                .Include(p => p.Utilizador)
                .Include(p => p.ListaComentarios).ThenInclude(c => c.Utilizador);
            
            // Executa a consulta e retorna a lista de posts para a view.
            return View(await listaPosts.ToListAsync());
        }

        // GET: Publicacoes/Criar
        // Este método exibe o formulário para criar uma nova publicação.
        [Authorize] // Garante que apenas utilizadores autenticados possam aceder a esta action.
        public IActionResult Criar(string? id, string? termo) // Parâmetros opcionais 'id' e 'termo' (para filtragem de categorias).
        {
            // Lê o arquivo 'doencas.json' para obter as categorias.
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
            // Desserializa o JSON para uma lista de objetos InfoCategoria.
            var categorias = JsonSerializer.Deserialize<List<InfoCategoria>>(jsonContent);

            // Filtra as categorias se um 'termo' de pesquisa for fornecido.
            if (!string.IsNullOrEmpty(termo))
            {
                termo = termo.ToLower(); // Converte para minúsculas para comparação case-insensitive.
                categorias = categorias
                    .Where(r =>
                        r.Categoria.ToLower().Contains(termo)) // Filtra por categorias que contêm o termo.
                    .ToList();
            }

            // Processa a lista de categorias para extrair categorias únicas e ordenadas para um dropdown.
            var categoriasDropdown = categorias
                .SelectMany(r => new[] { r.Categoria}) // Seleciona apenas o campo Categoria.
                .Distinct() // Remove duplicados.
                .OrderBy(x => x) // Ordena alfabeticamente.
                .ToList(); // Converte para lista.

            // Armazena a lista de categorias para o dropdown e o termo de pesquisa na ViewBag.
            ViewBag.Categorias = categoriasDropdown ?? new List<string>();
            ViewBag.Termo = termo;

            // Imprime o nome do utilizador autenticado na consola.
            Console.WriteLine("Current logged in user: " + User.Identity.Name);

            // Retorna a view para criar uma nova publicação.
            return View();
        }

        // POST: Publicacoes/Criar
        // Este método recebe os dados do formulário de criação de publicação.
        [HttpPost] // Indica que este método responde a requisições HTTP POST.
        [ValidateAntiForgeryToken] // Proteção contra ataques CSRF (Cross-Site Request Forgery).
        [Authorize] // Garante que apenas utilizadores autenticados possam submeter o formulário.
        public async Task<IActionResult> Criar(
            [Bind("TituloPost,TextoPost,Categoria,DataPost")] Posts post, // Vincula propriedades específicas do modelo 'Posts' aos dados do formulário.
            IFormFile imagem) // Recebe o arquivo de imagem enviado pelo utilizador.
        {
            bool haImagem = false; // Flag para indicar se uma imagem foi enviada e é válida.
            var nomeImagem = ""; // Variável para armazenar o nome único da imagem.

            // Obtém o utilizador autenticado do banco de dados.
            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);

            // Se o utilizador não for encontrado, adiciona um erro ao ModelState.
            if (utilizador == null)
            {
                ModelState.AddModelError("", "Utilizador não encontrado.");
            }
            
            post.Utilizador = utilizador; // Associa o utilizador encontrado ao post.
            post.IdUtil = utilizador.IdUtil; // Define o IdUtil do post.

            // Verifica se o estado do modelo é válido (validações do lado do servidor).
            if (ModelState.IsValid)
            {
                post.DataPost = DateOnly.FromDateTime(DateTime.Now); // Define a data da publicação como a data atual.
                
                // Verifica se uma imagem foi enviada e se é do tipo PNG ou JPEG.
                if (imagem != null && 
                    (imagem.ContentType == "image/png" || imagem.ContentType == "image/jpeg"))
                {
                    haImagem = true;
                    Guid g = Guid.NewGuid(); // Gera um GUID único para o nome do arquivo.
                    nomeImagem = g + Path.GetExtension(imagem.FileName).ToLowerInvariant(); // Concatena o GUID com a extensão original.
                    post.Foto = "imagens/" + nomeImagem; // Define o caminho da foto no modelo (relativo à wwwroot).
                }

                // Se houver uma imagem válida, salva-a no sistema de arquivos.
                if (haImagem)
                {
                    // Constrói o caminho completo para o diretório de imagens.
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                    // Cria o diretório se não existir.
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    filePath = Path.Combine(filePath, nomeImagem); // Constrói o caminho completo do arquivo.
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream); // Copia o conteúdo da imagem para o arquivo.
                    }
                }

                _context.Add(post); // Adiciona o novo post ao contexto do banco de dados.
                await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados.
                return RedirectToAction(nameof(Index)); // Redireciona para a action Index (lista de publicações).
            }

            // Se o ModelState não for válido (erros de validação), recarrega as categorias para o dropdown.
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/doencas.json");
            var categorias = JsonSerializer.Deserialize<List<InfoCategoria>>(jsonContent);
            var categoriasDropdown = categorias
                .SelectMany(r => new[] { r.Categoria })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Categorias = categoriasDropdown ?? new List<string>(); // Popula a ViewBag com as categorias.
            
            // Retorna a view 'Criar' com o objeto 'post' (contendo os dados submetidos e os erros).
            return View(post);
        }
        
        // GET: Publicacoes/Detalhes/5
        // Exibe os detalhes de uma publicação específica.
        public async Task<IActionResult> Detalhes(int? id) // 'id' é o ID do post a ser exibido.
        {
            if (id == null) return NotFound(); // Se o ID for nulo, retorna NotFound.

            // Busca o post no banco de dados, incluindo o utilizador associado.
            var post = await _context.Posts
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (post == null) return NotFound(); // Se o post não for encontrado, retorna NotFound.

            return View(post); // Retorna a view 'Detalhes' com o objeto 'post'.
        }
        
        // GET: Fotografias/Editar/5
        // Exibe o formulário para editar uma publicação existente.
        [Authorize] // Apenas utilizadores autenticados podem editar.
        public async Task<IActionResult> Editar(int? id) // 'id' é o ID do post a ser editado.
        {
            if (id == null)
            {
                return NotFound(); // Se o ID for nulo, retorna NotFound.
            }

            // Busca a publicação no banco de dados, incluindo o utilizador.
            var Publicacao =  _context.Posts
                .Include(f => f.Utilizador)
                .FirstOrDefault(f => f.IdPost == id);
            
            if (Publicacao == null)
            {
                return NotFound(); // Se a publicação não for encontrada, retorna NotFound.
            }

            // Verifica se o utilizador autenticado é o proprietário da publicação.
            if (Publicacao.Utilizador.IdentityUserName != User.Identity.Name)
            {
                // Se não for o proprietário, redireciona para a lista de publicações (Index).
                // Uma alternativa mais robusta seria retornar Forbid() ou uma view de "acesso negado".
                return RedirectToAction(nameof(Index)); 
            }
            
            // Retorna a view 'Editar' com o objeto 'Publicacao'.
            return View(Publicacao);
        }

        // POST: Fotografias/Edit/5
        // Processa a submissão do formulário de edição de publicação.
        [HttpPost] // Indica que este método responde a requisições HTTP POST.
        [ValidateAntiForgeryToken] // Proteção CSRF.
        [Authorize] // Apenas utilizadores autenticados podem editar.
        public async Task<IActionResult> Editar(int id,
            [Bind("IdPost,TituloPost,Categoria,TextoPost")] Posts publicacao, IFormFile imagem) // Vincula propriedades e recebe a nova imagem.
        {
            var haImagem = false; // Flag para nova imagem.
            var nomeImagem = ""; // Nome da nova imagem.
            
            // Verifica se o ID fornecido na URL corresponde ao ID do objeto enviado no formulário.
            if (id != publicacao.IdPost)
            {
                return NotFound(); // Se não corresponder, retorna NotFound.
            }

            // Se uma nova imagem for enviada e for válida (PNG/JPEG).
            if (imagem != null && 
                (imagem.ContentType == "image/png" || imagem.ContentType == "image/jpeg"))
            {
                haImagem = true;
                Guid g = Guid.NewGuid(); // Gera um novo GUID para o nome da nova imagem.
                nomeImagem = g + Path.GetExtension(imagem.FileName).ToLowerInvariant();
                publicacao.Foto = "imagens/" + nomeImagem; // Atualiza o caminho da foto no objeto 'publicacao'.
            }
            
            // Se houver uma nova imagem válida, salva-a no sistema de arquivos.
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
            
            // Busca a publicação existente no banco de dados para atualização.
            var publicacaoExistente = await _context.Posts
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (publicacaoExistente == null)
            {
                return NotFound(); // Se a publicação não for encontrada (concorrência ou ID inválido), retorna NotFound.
            }

            // Verifica se o utilizador autenticado é o proprietário da publicação.
            var user = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);

            if (user == null || publicacaoExistente.IdUtil != user.IdUtil)
            {
                return Forbid(); // Se o utilizador não for o proprietário, retorna Forbid (acesso negado).
            }

            // Se o estado do modelo for válido.
            if (ModelState.IsValid)
            {
                // Se uma nova imagem foi carregada, apaga a antiga (se existir) e atualiza o caminho da foto.
                if (haImagem)
                {
                    if (publicacaoExistente.Foto != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", publicacaoExistente.Foto);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    publicacaoExistente.Foto = publicacao.Foto; // Atualiza para o novo caminho da foto.
                }
                
                // Atualiza as outras propriedades do post existente com os valores do formulário.
                publicacaoExistente.TituloPost = publicacao.TituloPost;
                publicacaoExistente.Categoria = publicacao.Categoria;
                publicacaoExistente.TextoPost = publicacao.TextoPost;
                publicacaoExistente.DataPost = DateOnly.FromDateTime(DateTime.Now); // Atualiza a data do post para a data atual.
                
                try
                {
                    await _context.SaveChangesAsync(); // Tenta salvar as mudanças no banco de dados.
                }
                catch (DbUpdateConcurrencyException) // Captura exceções de concorrência (se o registro foi modificado por outro utilizador).
                {
                    if (!_context.Posts.Any(e => e.IdPost == publicacao.IdPost))
                    {
                        return NotFound(); // Se o post não existir mais, retorna NotFound.
                    }
                    else
                    {
                        throw; // Caso contrário, relança a exceção.
                    }
                }

                return RedirectToAction(nameof(Index)); // Redireciona para a lista de publicações após a edição.
            }

            // Se o ModelState não for válido, retorna a view 'Editar' com o objeto 'publicacaoExistente'
            // para que os erros possam ser exibidos.
            return View(publicacaoExistente);
        }
        
        // GET: Publicacoes/Delete/5
        // Exibe a página de confirmação de exclusão de uma publicação.
        [Authorize] // Apenas utilizadores autenticados podem aceder a esta página.
        public async Task<IActionResult> Delete(int? id) // 'id' é o ID do post a ser apagado.
        {
            if (id == null) return NotFound(); // Se o ID for nulo, retorna NotFound.

            // Busca o post no banco de dados, incluindo o utilizador associado.
            var post = await _context.Posts
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.IdPost == id);

            if (post == null) return NotFound(); // Se o post não for encontrado, retorna NotFound.

            return View(post); // Retorna a view 'Delete' com o objeto 'post'.
        }

        // POST: Publicacoes/Delete/5
        // Processa a exclusão de uma publicação.
        [HttpPost, ActionName("Delete")] // Responde a POSTs, mas a action é nomeada "Delete" (para chamar com URL /Delete).
        [ValidateAntiForgeryToken] // Proteção CSRF.
        [Authorize] // Apenas utilizadores autenticados podem apagar.
        public async Task<IActionResult> DeleteConfirmed(int id) // 'id' é o ID do post a ser apagado.
        {
            // Busca o post no banco de dados pelo ID.
            var post = await _context.Posts.FindAsync(id);

            // Se o post for encontrado.
            if (post != null)
            {
                // Se o post tiver uma imagem associada, tenta apagá-la do sistema de arquivos.
                if (post.Foto != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", post.Foto);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Posts.Remove(post); // Remove o post do contexto.
                await _context.SaveChangesAsync(); // Salva as mudanças (executa a exclusão no banco de dados).
            }

            return RedirectToAction(nameof(Index)); // Redireciona para a lista de publicações após a exclusão.
        }
        
        // Classe interna pública para desserializar as informações de categoria de um arquivo JSON.
        public class InfoCategoria
        {
            // Atributos JsonPropertyName mapeiam as propriedades C# para os nomes dos campos JSON.
            [JsonPropertyName("nome")] 
            public string Nome { get; set; } // Nome da doença (no contexto do JSON de doenças).
            [JsonPropertyName("categoria")] 
            public string Categoria { get; set; } // Categoria da doença.
        }
    }
}