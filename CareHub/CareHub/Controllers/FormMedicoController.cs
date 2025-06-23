using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CareHub.Data;
using CareHub.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Controllers
{
    [Authorize]
    public class FormMedicoController : Controller
    {
        private readonly ApplicationDbContext _context;


        
        public FormMedicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formulário
        public IActionResult FormMedico()
        {
            var identityUserName = User.Identity.Name;

            var utilizador = _context.Utilizadores
                .Include(u => u.Doutor)
                .FirstOrDefault(u => u.IdentityUserName == identityUserName);

            if (utilizador == null)
                return Unauthorized();

            if (utilizador.Doutor != null)
            {
                // Se já existe doutor, mostra aviso pendente
                ViewBag.MensagemTitulo = "A aguardar resposta";
                ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
                return View("Aviso");
            }

            // Se não tiver doutor, mostra formulário
            var vm = new FormMedicoViewModel
            {
                Doutor = new Doutores { IdUtil = utilizador.IdUtil },
                Regioes = CarregarRegioes()
            };

            return View(vm);
        }

        private List<string> CarregarRegioes()
        {
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);

            return regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
// POST: Submeter o formulário do doutor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FormMedico(Doutores doutor)
        {
            var identityUserName = User.Identity.Name;

            var utilizador = _context.Utilizadores
                .Include(u => u.Doutor)
                .FirstOrDefault(u => u.IdentityUserName == identityUserName);

            if (utilizador == null)
                return Unauthorized();

            if (utilizador.Doutor != null)
            {
                ModelState.AddModelError("", "Já existe um registo de doutor para este utilizador.");
            }
            else
            {
                doutor.IdUtil = utilizador.IdUtil;
                _context.Doutores.Add(doutor);
                _context.SaveChanges();

                ViewBag.MensagemTitulo = "O seu registo como doutor foi submetido com sucesso!";
                ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
                return View("Aviso");
            }

            // Em caso de erro, voltar ao formulário
            var vm = new FormMedicoViewModel
            {
                Doutor = doutor,
                Regioes = CarregarRegioes()
            };

            return View(vm);
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
        
        public class FormMedicoViewModel
        {
            public Doutores Doutor { get; set; }
            public Utilizadores Utilizador { get; set; }
            public List<string> Regioes { get; set; }
        }

    }
}