
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Para interações com o Entity Framework Core e a base de dados.
using CareHub.Data; // O namespace para applicationdbcontext
using CareHub.Models; // O namespace para os modelos de dados da aplicação (e.g., Utilizadores, Posts).
using Microsoft.AspNetCore.Authorization; // Para controlo de acesso baseado em autorização.
using Microsoft.AspNetCore.Identity; // Para gerenciar utilizadores e roles do ASP.NET Core Identity.


namespace CareHub.Controllers // Declara o namespace do controller.
{
    [Authorize(Roles = "Administrador")] // Atributo de autorização: Apenas utilizadores com a role "Administrador" podem aceder qualquer action neste controller.
    public class UtilizadoresController : Controller // Define a classe UtilizadoresController
    {
        private readonly ApplicationDbContext _context; // variavel a instância do applicationdbcontext.
        private readonly UserManager<IdentityUser> _userManager; // variavel para o UserManager, usado para gerir utilizadores do Identity.

        // Construtor do controller.
        // Recebe instâncias de ApplicationDbContext e UserManager.
        public UtilizadoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context; // Atribui o contexto da base de dados.
            _userManager = userManager; // Atribui o UserManager.
        }

        // GET: Utilizadores
        // Action para exibir a lista de utilizadores.
        public async Task<IActionResult> Index()
        {
            // Retorna uma View com uma lista de todos os utilizadores na base de dados.
            return View(await _context.Utilizadores.ToListAsync());
        }

        // GET: Utilizadores/Detalhes/5
        // Action para exibir os detalhes de um utilizador específico.
        public async Task<IActionResult> Detalhes(int? id) // 'id' é o ID do utilizador.
        {
            if (id == null) // Se o ID for nulo, retorna NotFound (erro 404).
            {
                return NotFound();
            }

            // procura o utilizador na base de dados pelo ID, incluindo a lista de posts associados.
            var utilizador = await _context.Utilizadores
                .Include(u=>u.ListaPosts) // Carrega os posts relacionados ao utilizador.
                .FirstOrDefaultAsync(m => m.IdUtil == id); // Encontra o primeiro utilizador com o ID correspondente.
            if (utilizador == null) // Se o utilizador não for encontrado, retorna NotFound.
            {
                return NotFound();
            }

            return View(utilizador); // Retorna a View com o objeto utilizador.
        }

        // GET: Utilizadores/Editar/5
        // Action para exibir o formulário de edição de um utilizador.
        public async Task<IActionResult> Editar(int? id) // 'id' é o ID do utilizador a ser editado.
        {
            if (id == null) // Se o ID for nulo, retorna NotFound.
            {
                return NotFound();
            }

            // Busca o utilizador no banco de dados pelo ID.
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null) // Se o utilizador não for encontrado, retorna NotFound.
            {
                return NotFound();
            }
            // Guarda o ID do utilizador na sessão HTTP. Isso é uma medida de segurança para
            // verificar se o utilizador está realmente a editar o mesmo utilizador que visualizou.
            HttpContext.Session.SetInt32("utilizadorId", utilizador.IdUtil);
            
            return View(utilizador); // Retorna a View com o objeto utilizador.
        }

        // POST: Utilizadores/Editar/5
        // Action para processar a submissão do formulário de edição de utilizador.
        [HttpPost] // Indica que este método responde a requisições HTTP POST.
        [ValidateAntiForgeryToken] // Proteção contra ataques Cross-Site Request Forgery (CSRF).
        // [FromRoute]int id: Obtém o ID do utilizador da rota.
        // [Bind(...)] Utilizadores utilizadores: Vincula as propriedades especificadas do modelo 'Utilizadores' aos dados do formulário.
        public async Task<IActionResult> Editar([FromRoute]int id, [Bind("IdUtil,Nome,NIF,Telemovel,Morada,CodPostal,Pais")] Utilizadores utilizadores)
        {
            // Verifica se o ID da rota corresponde ao ID do modelo recebido.
            // Isso ajuda a evitar que um atacante tente editar um utilizador diferente manipulando o ID no formulário.
            if (id != utilizadores.IdUtil)
            {
                return NotFound(); // Se os IDs não corresponderem, retorna NotFound.
            }

            // Verifica se as validações do modelo foram bem-sucedidas.
            if (ModelState.IsValid)
            {
                try
                {
                    // Recupera o ID do utilizador armazenado na sessão.
                    var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                    // Validação de segurança: compara o ID da sessão com o ID recebido.
                    // Se forem diferentes, indica uma tentativa de manipulação.
                    if (utilizadorDaSessao != id)
                    {
                        ModelState.AddModelError("", "Tentaste aldrabar isto palhaço! Outra vez!!!"); // Mensagem de erro.
                        return View(utilizadores); // Retorna a View com o erro.
                    }
                    
                    // Atualiza o objeto utilizadores no applicationdbcontext.
                    _context.Update(utilizadores);
                    // guarda as mudanças assincronamente na base de dados.
                    await _context.SaveChangesAsync();
                    // Limpa o ID do utilizador da sessão para evitar POSTs sucessivos com o mesmo ID.
                    HttpContext.Session.SetInt32("utilizadorId", 0);
                }
                catch (DbUpdateConcurrencyException) // se o registro foi modificado por outro utilizador simultaneamente.
                {
                    if (!UtilizadoresExists(utilizadores.IdUtil)) // Verifica se o utilizador ainda existe.
                    {
                        return NotFound(); // Se não existir, retorna NotFound.
                    }
                    else
                    {
                        throw; // Caso contrário, relança a exceção.
                    }
                }
                return RedirectToAction(nameof(Index)); // Redireciona para a Action Index após a edição.
            }
            return View(utilizadores); // Se o ModelState não for válido, retorna a View com o objeto utilizadores para exibir os erros.
        }

        // GET: Utilizadores/Delete/5
        // Action para exibir a página de confirmação de exclusão de um utilizador.
        public async Task<IActionResult> Delete(int? id) // 'id' é o ID do utilizador a ser apagado.
        {
            if (id == null) // Se o ID for nulo, retorna NotFound.
            {
                return NotFound();
            }
            
            // Busca o utilizador na base de dados.
            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.IdUtil == id);
            if (utilizadores == null) // Se o utilizador não for encontrado, retorna NotFound.
            {
                return NotFound();
            }
            
            // Guarda o ID do utilizador na sessão, parecido ao Editar, para validação no POST.
            HttpContext.Session.SetInt32("utilizadorId", utilizadores.IdUtil);

            return View(utilizadores); // Retorna a View com o objeto utilizador.
        }

        // POST: Utilizadores/Delete/5
        // Action para processar a exclusão confirmada de um utilizador.
        [HttpPost, ActionName("Delete")] // Responde a POSTs, e o nome da action para URL é "Delete".
        [ValidateAntiForgeryToken] // Proteção CSRF.
        public async Task<IActionResult> DeleteConfirmed(int id) // 'id' é o ID do utilizador a ser apagado.
        {
            // Busca o utilizador na base de dados, incluindo os seus posts associados.
            var utilizador = await _context.Utilizadores.Include(p => p.ListaPosts).FirstAsync(u => u.IdUtil == id);
            
            if (utilizador != null) // Se o utilizador for encontrado.
            {
                // Validação de segurança com o ID da sessão.
                var utilizadorDaSessao = HttpContext.Session.GetInt32("utilizadorId");
                if (utilizadorDaSessao != id)
                {
                    // Se houver tentativa de manipulação, redireciona para a Index sem apagar.
                    return RedirectToAction(nameof(Index));
                }
                
                // Remove as fotos associadas aos posts do utilizador.
                foreach (var item in utilizador.ListaPosts)
                {
                    string localImagem = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + item.Foto);
                    if (System.IO.File.Exists(localImagem)) // Verifica se o arquivo existe antes de tentar apagar.
                    {
                        System.IO.File.Delete(localImagem); // Apaga o arquivo da imagem.
                    }
                }

                // Remove a foto de perfil do utilizador, a menos que seja a imagem padrão "user.jpg".
                string imagemUtilizador = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+utilizador.Foto);
                string nomeFoto = System.IO.Path.GetFileName(imagemUtilizador);
                if (!nomeFoto.Equals("user.jpg", StringComparison.OrdinalIgnoreCase)) // Compara ignorando maiúsculas/minúsculas.
                {
                    System.IO.File.Delete(imagemUtilizador); // Apaga a imagem do utilizador.
                }
               
                // Obtém o objeto IdentityUser associado ao utilizador da aplicação.
                IdentityUser userIdentity = await _userManager.FindByEmailAsync(utilizador.IdentityUserName);
                // Apaga o utilizador do ASP.NET Core Identity.
                _userManager.DeleteAsync(userIdentity);
                
                // Remove o utilizador da applicationdbcontext
                _context.Utilizadores.Remove(utilizador);
            }
            
            await _context.SaveChangesAsync(); // guarda as mudanças na base de dados (exclui o utilizador e os seus posts).
            // Limpa o ID do utilizador da sessão.
            HttpContext.Session.SetInt32("utilizadorId",0);
            return RedirectToAction(nameof(Index)); // Redireciona para a Action Index.
        }

        // Método auxiliar para verificar se um utilizador existe na base de dados.
        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.IdUtil == id); // Retorna true se encontrar algum utilizador com o ID.
        }
    }
}