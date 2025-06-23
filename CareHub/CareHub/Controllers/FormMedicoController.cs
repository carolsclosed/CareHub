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
        public IActionResult FormMedico(string? termo)
        {
            var identityUserName = User.Identity.Name;

            var utilizador = _context.Utilizadores
                .Include(u => u.Doutor)
                .FirstOrDefault(u => u.IdentityUserName == identityUserName);

            if (utilizador == null)
                return Unauthorized();

            // Se já existe doutor para este utilizador, mostra aviso logo aqui
            if (utilizador.Doutor != null)
            {
                return View("Aviso");  // View aviso a informar que já existe registo
            }

            // Caso contrário, carrega as regiões e mostra o formulário
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var doutor = new Doutores
            {
                IdUtil = utilizador.IdUtil
            };

            var vm = new FormMedicoViewModel
            {
                Doutor = doutor,
                Regioes = regioesDropdown
            };

            return View(vm);
        }

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
                // Já existe doutor -> volta para aviso
                return View("Aviso");
            }

            doutor.IdUtil = utilizador.IdUtil;

            if (!ModelState.IsValid)
            {
                var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
                var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
                var regioesDropdown = regioes
                    .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                var vm = new FormMedicoViewModel
                {
                    Doutor = doutor,
                    Regioes = regioesDropdown
                };

                return View(vm);
            }

            _context.Doutores.Add(doutor);
            _context.SaveChanges();

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
        
        public class FormMedicoViewModel
        {
            public Doutores Doutor { get; set; }
            public Utilizadores Utilizador { get; set; }
            public List<string> Regioes { get; set; }
        }

    }
}