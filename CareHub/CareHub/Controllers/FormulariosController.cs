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



        public async Task<IActionResult> onlineForm(string? id)
        {
         return View("onlineForm");   
        }


        public async Task<IActionResult> presencialForm(string? id)
        {
            return View("presencialForm");   
        }
        
        // POST: Recebe os dados
        [HttpPost]
        public IActionResult onlineForm(Models.Formularios form)
        {
            form.presencial = false;

            
            var identityUserName = User.Identity.Name;

            // procura o utilizador 
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == identityUserName);
    
            if (utilizador == null)
            {
                return Unauthorized(); // Ou redireciona para login
            }

            // Associa o ID do utilizador ao formulário
            form.IdUtil = utilizador.IdUtil;

            _context.Add(form);
            _context.SaveChanges();
            
            return View(); // ou outra página
        }
        
        // POST: Recebe os dados
        [HttpPost]
        public IActionResult presencialForm(Models.Formularios form)
        {
            form.presencial = true;
            
            var identityUserName = User.Identity.Name;

            // procura o utilizador 
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.IdentityUserName == identityUserName);
    
            if (utilizador == null)
            {
                return Unauthorized(); // Ou redireciona para login
            }

            // Associa o ID do utilizador ao formulário
            form.IdUtil = utilizador.IdUtil;

            _context.Add(form);
            _context.SaveChanges();
            
            return View(); // ou outra página
        }

}