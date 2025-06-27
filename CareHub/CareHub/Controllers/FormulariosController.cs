using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;                // <- necessário para NormalizationForm
using System.Globalization;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Authorization; // <- necessário para CharUnicodeInfo


namespace CareHub.Controllers;
[Authorize]
public class FormulariosController :  Controller
{
        private readonly ApplicationDbContext _context;

        public FormulariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Formulário
        public IActionResult Criar()
        {
            return View("Criar");
        }



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

                // Junta todas as strings únicas (nome, distrito, provincia)
                var regioesDropdown = regioes
                    .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                ViewBag.Regioes = regioesDropdown ?? new List<string>();
                ViewBag.Termo = termo;

                return View("onlineForm");

        }


        public async Task<IActionResult> presencialForm(string? id , string? termo)
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

            // Junta todas as strings únicas (nome, distrito, provincia)
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Regioes = regioesDropdown ?? new List<string>();
            ViewBag.Termo = termo;

            return View("presencialForm");
 
        }
        
        // POST: Recebe os dados
        [HttpPost]
        public IActionResult onlineForm(Models.Formularios form)
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
                _context.Add(form);
                _context.SaveChanges();

                // Redirecione para uma página de sucesso ou outra ação
                return RedirectToAction("Obrigado"); 
            }

            // Se ModelState não é válido, popula ViewBag.Regioes para a view
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Regioes = regioesDropdown ?? new List<string>();

            return View(form); // retorna a view com o model e ViewBag preenchidos
        }

        
        // POST: Recebe os dados
        [HttpPost]
        public IActionResult presencialForm(Models.Formularios form)
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
                _context.Add(form);
                _context.SaveChanges();
            }

            // Se o ModelState não é válido, repopular regioes para mostrar a view
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);
            var regioesDropdown = regioes
                .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.Regioes = regioesDropdown ?? new List<string>();

            return View(form); // retorna a view com o model e ViewBag preenchido
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