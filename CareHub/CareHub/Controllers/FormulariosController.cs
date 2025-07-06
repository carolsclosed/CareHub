using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using System.Threading.Tasks;
using CareHub.Data; // Importa o namespace CareHub.Data
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Authorization; // Importa o namespace Microsoft.AspNetCore.Authorization, usado para controlo de acesso e autorização.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers; // Declara o namespace para o controller.
[Authorize] // Atributo que garante que apenas utilizadores autenticados podem aceder a qualquer action deste controller.
public class FormulariosController : Controller // Declara a classe FormulariosController
{
    private readonly ApplicationDbContext _context; // Declara uma variavel  para a instância applicationdbcontext.

    private readonly IMailer _mailer;
    
    
    // Construtor da classe FormulariosController.
    // Recebe uma instância de ApplicationDbContext.
    // A instância é atribuída a variavel _context, permitindo a interação com a base de dados.
    public FormulariosController(ApplicationDbContext context, IMailer mailer)
    {
        _context = context;
        _mailer = mailer;
    }

    // GET: Formulário
    // action method que responde a requisições HTTP GET para exibir a view "Criar".
    public IActionResult Criar()
    {
        return View("Criar"); // Retorna a view nomeada "Criar".
    }

    // GET: onlineForm
    // action method que responde a requisições HTTP GET para exibir o formulário tipo online.
    // Aceita parâmetros opcionais 'id' e 'termo' (para pesquisa/filtragem).
    public async Task<IActionResult> onlineForm(string? id, string? termo)
    {
        // lê o json regioes.json na pasta wwwroot
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
        // Desserializa o conteúdo JSON para uma lista de objetos InfoRegiao.
        var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);

        // Se um 'termo' de pesquisa for fornecido, filtra as regiões.
        if (!string.IsNullOrEmpty(termo))
        {
            termo = termo.ToLower(); // Converte o termo para minúsculas para comparação case-insensitive.
            regioes = regioes
                .Where(r => // Filtra as regiões onde qualquer um dos campos (Provincia, Distritos, Regioes, Nome) contém o termo.
                    r.Provincia.ToLower().Contains(termo) ||
                    r.Distritos.ToLower().Contains(termo) ||
                    r.Regioes.ToLower().Contains(termo) ||
                    r.Nome.ToLower().Contains(termo))
                .ToList(); // Converte o resultado para uma lista.
        }

        // Processa a lista de objetos InfoRegiao para extrair todos os nomes, distritos, províncias e regiões
        // como uma única lista de strings distintas e ordenadas. 
        var regioesDropdown = regioes
            .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes }) // Acha todos os campos relevantes para cada região.
            .Distinct() // Remove duplicados.
            .OrderBy(x => x) // Ordena alfabeticamente.
            .ToList(); // Converte o resultado para uma lista.

        // Armazena a lista de regiões filtradas (para o dropdown) na ViewBag.
        // Se regioesDropdown for null, inicializa uma lista vazia.
        ViewBag.Regioes = regioesDropdown ?? new List<string>();
        // Armazena o termo de pesquisa na ViewBag para que possa ser exibido na view.
        ViewBag.Termo = termo;

        // Retorna a view nomeada "onlineForm".
        return View("onlineForm");
    }

    // GET: presencialForm
    //action method que responde a requisições HTTP GET para exibir o formulário tipo presencial.
    // É muito similar ao onlineForm, diferindo apenas na view que retorna.
    public async Task<IActionResult> presencialForm(string? id, string? termo)
    {
        var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
        var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);

        if (!string.IsNullOrEmpty(termo))
        {
            termo = termo.ToLower();
            regioes = regioes
                .Where(r =>
                    r.Provincia.ToLower().Contains(termo) ||
                    r.Distritos.ToLower().Contains(termo) ||
                    r.Regioes.ToLower().Contains(termo) ||
                    r.Nome.ToLower().Contains(termo))
                .ToList();
        }

        var regioesDropdown = regioes
            .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        ViewBag.Regioes = regioesDropdown ?? new List<string>();
        ViewBag.Termo = termo;

        return View("presencialForm");
    }
    
    // POST: Recebe os dados do formulário
    //action method que responde a requisições HTTP POST quando o formulário tipo online é submetido.
    [HttpPost]
    public async Task<IActionResult> onlineForm(Models.Formularios form) // O objeto 'form' é preenchido automaticamente com os dados do formulário.
    {
        form.presencial = false; // Define a propriedade 'presencial' como falso, indicando que é um formulário para consulta online.

        // Obtém o nome do utilizador atualmente autenticado.
        var identityUserName = User.Identity.Name;
        // Procura o utilizador na base de dados com base no IdentityUserName.
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == identityUserName);

        // Se o utilizador não for encontrado, retorna um status de não autorizado.
        if (utilizador == null)
        {
            return Unauthorized();
        }

        form.IdUtil = utilizador.IdUtil; // Associa o formulário ao Id do utilizador que está autenticado.

        // Verifica se todas as validações do modelo foram bem-sucedidas.
        if (ModelState.IsValid)
        {
            
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Regioes = regioesDropdown ?? new List<string>(); // preenche a ViewBag.Regioes para a view.

            var emailCorpo = "";
            //preparar o email
            if (form.presencial == false)
            {
                emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Falso <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
            }
            else
            {
                emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Verdadeiro<br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
            }
            
            var emailAssunto = "Confirmação do formulário";

            try
            {
                
                //enviar email de confirmação
                await _mailer.SendEmailAsync(form.email, emailAssunto, emailCorpo);
                ViewBag.MensagemCorpo =
                    "Obrigada por efetuar a submissão irá receber um email de confirmação brevemente";
                _context.Add(form); // Adiciona o objeto 'form' ao contexto para ser inserido na base de dados.
                await _context.SaveChangesAsync(); // guarda as mudanças na base de dados.
                
            }
            catch (Exception ex)
            {
                ViewBag.MensagemCorpo =
                    "Ocurreu um erro na submissão por favor tente novamente mais tarde";
            }

        }
        else
        {
            ViewBag.MensagemCorpo = "Dados Inválidos. Por favor tente novamente";

        }

        // Retorna a view "Aviso" onde vai ser exibida as mensagens.
        return View("Aviso");
    }

    // POST: Recebe os dados do formulário para consulta presencial.
    //  action method que responde a requisições HTTP POST quando o formulário presencial é submetido.
    //praticamente igual ao anterior so muda a view
    [HttpPost]
    public async Task<IActionResult> presencialForm(Models.Formularios form) 
    {
        form.presencial = true; 
    
       
        var identityUserName = User.Identity.Name;
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == identityUserName);

        if (utilizador == null)
        {
            return Unauthorized();
        }

        form.IdUtil = utilizador.IdUtil; 
        if (ModelState.IsValid)
        {

            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Regioes = regioesDropdown ?? new List<string>();
            
            var emailCorpo = "";
            //preparar o email
            if (form.presencial == false)
            {
                emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Falso <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
            }
            else
            {
                emailCorpo = $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Verdadeiro<br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
            }
            
            var emailAssunto = "Confirmação do formulário";

            try
            {

                //enviar email de confirmação
                await _mailer.SendEmailAsync(form.email, emailAssunto, emailCorpo);
                ViewBag.MensagemCorpo =
                    "Obrigada por efetuar a submissão irá receber um email de confirmação brevemente";
                _context.Add(form); // Adiciona o objeto 'form' ao contexto para ser inserido na base de dados.
                await _context.SaveChangesAsync(); // guarda as mudanças na base de dados.

            }
            catch (Exception ex)
            {
                ViewBag.MensagemCorpo =
                    "Ocurreu um erro na submissão por favor tente novamente mais tarde";
            }
            

        }
        else
        {
            ViewBag.MensagemCorpo = "Dados Inválidos. Por favor tente novamente.";

        }
        return View("Aviso");

    }
    


    // Classe interna  para desserializar as informações de região de um arquivo JSON.
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
}