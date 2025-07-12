using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CareHub.Data;
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareHub.Controllers;
/// <summary>
/// controller para os formulários de consulta presenciais e online
/// </summary>
[Authorize]
public class FormulariosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMailer _mailer;

    public FormulariosController(ApplicationDbContext context, IMailer mailer)
    {
        _context = context;
        _mailer = mailer;
    }

    public IActionResult Criar()
    {
        return View("Criar");
    }

    /// <summary>
    /// Método para o formulário de consulta online
    /// </summary>
    /// <param name="id"></param>
    /// <param name="termo"></param>
    /// <returns></returns>
    public async Task<IActionResult> onlineForm(string? id, string? termo)
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

        return View("onlineForm");
    }

    
    /// <summary>
    /// Método para o formulário de consulta presencial
    /// </summary>
    /// <param name="id"></param>
    /// <param name="termo"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Método de submissão do formulário de consulta online
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> onlineForm(Models.Formularios form)
    {
        form.presencial = false;

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

            var emailCorpo = form.presencial
                ? $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Sim <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub"
                : $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Não <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";

            var emailAssunto = "Confirmação do formulário";

            try
            {
                await _mailer.SendEmailAsync(form.email, emailAssunto, emailCorpo);
                ViewBag.MensagemCorpo =
                    "Obrigada por efetuar a submissão irá receber um email de confirmação brevemente";
                _context.Add(form);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ViewBag.MensagemCorpo =
                    "Ocurreu um erro na submissão por favor tente novamente mais tarde";
            }
        }
        else
        {
            ViewBag.MensagemCorpo = "Dados Inválidos. Por favor tente novamente";
        }

        return View("Aviso");
    }

    /// <summary>
    /// Método de submissão do formulário de consulta presencial
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
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

            var emailCorpo = form.presencial
                ? $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Verdadeiro<br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub"
                : $"Olá {form.nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos um doutor disponíve para agendar uma consulta online. <br><br>Detalhes: <br><br>Descrição: {form.descricao}<br><br>Presencial: Falso <br><br>Região: {form.regiao}<br><br>Telefone: {form.telefone}<br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";

            var emailAssunto = "Confirmação do formulário";

            try
            {
                await _mailer.SendEmailAsync(form.email, emailAssunto, emailCorpo);
                ViewBag.MensagemCorpo =
                    "Obrigada por efetuar a submissão irá receber um email de confirmação brevemente";
                _context.Add(form);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
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

    public class InfoRegiao
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; }
        [JsonPropertyName("district")]
        public string Distritos { get; set; }
        [JsonPropertyName("region")]
        public string Regioes { get; set; }
        [JsonPropertyName("province")]
        public string Provincia { get; set; }
    }
}