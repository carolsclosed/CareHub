using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.AspNetCore.Authorization; // Importa o namespace Microsoft.AspNetCore.Authorization, usado para controlo de acesso e autorização.
using CareHub.Data; // Importa o namespace CareHub.Data
using CareHub.Models; // Importa o namespace CareHub.Models
using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using Microsoft.EntityFrameworkCore; // Importa o namespace Microsoft.EntityFrameworkCore, que fornece classes e funcionalidades para trabalhar com o Entity Framework Core (ORM).

namespace CareHub.Controllers // Declara o namespace para o controller.
{
    [Authorize] // Atributo que garante que apenas utilizadores autenticados possam aceder a qualquer action deste controller.
    public class FormMedicoController : Controller // Declara a classe FormMedicoController
    {
        private readonly ApplicationDbContext _context; // Declara uma variavel para a instância applicationdbcontext.

        // Construtor da classe FormMedicoController.
        // Recebe uma instância de ApplicationDbContext.
        // A instância é atribuída a variavel _context para poder interagir com a base de dados.
        public FormMedicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formulário
        // action method que responde a requisições HTTP GET para exibir o formulário de registo de médico.
        public IActionResult FormMedico()
        {
            // Obtém o nome de utilizador do utilizador atualmente autenticado.
            var identityUserName = User.Identity.Name;

            // Procura o utilizador na base de dados, incluindo os dados do 'Doutor' associado (se existir).
            // O .Include(u => u.Doutor) carrega o objeto 'Doutor' relacionado.
            // O .FirstOrDefault() obtém o primeiro utilizador que corresponde ao IdentityUserName, ou null se não encontrar.
            var utilizador = _context.Utilizadores
                .Include(u => u.Doutor)
                .FirstOrDefault(u => u.IdentityUserName == identityUserName);

            // Se o utilizador não for encontrado na base de dados, retorna um status de não autorizado.
            if (utilizador == null)
                return Unauthorized();

            // Verifica se o utilizador já tem um registo de 'Doutor' associado.
            if (utilizador.Doutor != null)
            {
                // Se já existir um registo de doutor diz que está a aguardar resposta pra ser aceite
                // Se não existir diz que agradece o seu interesse na carehub
                ViewBag.MensagemTitulo = "A aguardar resposta";
                ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
                // Retorna a view "Aviso" onde vai ser exibida as mensagens.
                return View("Aviso");
            }

            // Se o utilizador não tiver um registo de doutor, prepara o modelo da view para exibir o formulário.
            var vm = new FormMedicoViewModel
            {
                // Inicializa um novo objeto Doutores e associa o IdUtil do utilizador atual.
                Doutor = new Doutores { IdUtil = utilizador.IdUtil },
                // Carrega a lista de regiões através do método auxiliar CarregarRegioes().
                Regioes = CarregarRegioes()
            };

            // passamos o FormMedicoViewModel como modelo para a view
            return View(vm);
        }

        // Método auxiliar para carregar a lista de regiões de um arquivo JSON.
        private List<string> CarregarRegioes()
        {
            // lê o json da regioes na pasta wwwroot
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            
            // Desserializa o conteúdo JSON para uma lista de objetos InfoRegiao.
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);

            // Processa a lista de objetos InfoRegiao para extrair todos os nomes, distritos, províncias e regiões
            // como uma única lista de strings distintas.
            return regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes }) // encontra todos os campos relevantes para cada região.
                .Distinct() // Remove duplicados.
                .OrderBy(x => x) // Ordena alfabeticamente.
                .ToList(); // Converte o resultado para uma lista.
        }

        // POST: Submete o formulário do doutor
        //action method que responde a requisições HTTP POST para submeter os dados do formulário do médico.
        [HttpPost] // especifica que este método responde a requisições HTTP POST.
        [ValidateAntiForgeryToken] // Atributo de segurança que ajuda a prevenir ataques CSRF (Cross-Site Request Forgery).
        public IActionResult FormMedico(Doutores doutor) // O parâmetro 'doutor' é preenchido automaticamente com os dados do formulário.
        {
            // Obtém o nome de utilizador do utilizador atualmente autenticado.
            var identityUserName = User.Identity.Name;

            // Procura o utilizador na base de dados, incluindo os dados do 'Doutor' associado (se existir).
            var utilizador = _context.Utilizadores
                .Include(u => u.Doutor)
                .FirstOrDefault(u => u.IdentityUserName == identityUserName);

            // Se o utilizador não for encontrado, retorna um status de não autorizado.
            if (utilizador == null)
                return Unauthorized();

            // Verifica novamente se o utilizador já tem um registo de 'Doutor' associado para evitar submissões duplicadas.
            if (utilizador.Doutor != null)
            {
                // Se já existir, adiciona um erro ao ModelState, que será exibido na view.
                ModelState.AddModelError("", "Já existe um registo de doutor para este utilizador.");
            }
            else
            {
                // Se não houver registo de doutor, associa o IdUtil do utilizador ao novo objeto Doutores.
                doutor.IdUtil = utilizador.IdUtil;
                // Adiciona o novo objeto Doutores a base de dados.
                _context.Doutores.Add(doutor);
                // guarda as mudanças na base de dados.
                _context.SaveChanges();

                // Define mensagens de sucesso para a ViewBag.
                ViewBag.MensagemTitulo = "O seu registo como doutor foi submetido com sucesso!";
                ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
                // Retorna a view "Aviso" para informar o utilizador sobre o sucesso da submissão.
                return View("Aviso");
            }

            // Se o ModelState não for válido (houveram erros de validação ou a condição de "doutor já existe" foi atendida),
            // retorna o formulário com os dados submetidos e os erros.
            var vm = new FormMedicoViewModel
            {
                Doutor = doutor, // Mantém os dados submetidos pelo utilizador.
                Regioes = CarregarRegioes() // Recarrega as regiões para preencher o dropdown.
            };

            // Retorna a view "FormMedico" novamente, permitindo que o utilizador corrija os erros.
            return View(vm);
        }

        // Classe interna para desserializar as informações de região do JSON.
        public class InfoRegiao
        {
            // Atributos JsonPropertyName mapeiam as propriedades para os nomes dos campos JSON.
            [JsonPropertyName("name")] 
            public string Nome { get; set; } // Nome da região/localidade.
            [JsonPropertyName("district")] 
            public string Distritos { get; set; } // Distrito.
            [JsonPropertyName("region")] 
            public string Regioes { get; set; } // Região (macro).
            [JsonPropertyName("province")] 
            public string Provincia { get; set; } // Província.
        }
        
        // ViewModel para combinar múltiplos modelos e dados para a view FormMedico.
        public class FormMedicoViewModel
        {
            public Doutores Doutor { get; set; } // O objeto Doutores que será preenchido pelo formulário.
            public Utilizadores Utilizador { get; set; } // Dados do utilizador
            public List<string> Regioes { get; set; } // Lista de strings para mostrar um dropdown de regiões no formulário.
        }
    }
}