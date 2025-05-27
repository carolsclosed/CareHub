// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        
        public IndexModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public Utilizadores Utilizador { get; set; }

        public string Username { get; set; }
        
        public string Foto { get; set; }
        
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Número de Telefone")]
            public string PhoneNumber { get; set; }
            
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            public IFormFile Foto { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        
            // Get Utilizador data
            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == userName);

            Username = utilizador.Nome;
            Utilizador = utilizador;
            Foto = utilizador.Foto;
            
            Input = new InputModel
            {
                PhoneNumber = utilizador?.Telefone ?? phoneNumber,
                Nome = utilizador?.Nome
            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o user ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o user ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.IdentityUserName == user.UserName);

            if (utilizador != null)
            {
                
                utilizador.Telefone = Input.PhoneNumber;
                if (Input.Foto != null && (Input.Foto.ContentType == "image/png" || Input.Foto.ContentType == "image/jpeg"))
                {
                    
                    string pastaFicheiro = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/imagensUtilizadores");
                    string nomefoto =  Path.Combine("/imagensUtilizadores/",Guid.NewGuid().ToString() + Path.GetExtension(Input.Foto.FileName).ToLower()) ;
                    
                    
                    if (!Directory.Exists(pastaFicheiro))
                    {
                        Directory.CreateDirectory(pastaFicheiro);
                    }
            
                   
                    using (var fileStream = new FileStream(Directory.GetCurrentDirectory()+"/wwwroot"+nomefoto, FileMode.Create))
                    {
                        await Input.Foto.CopyToAsync(fileStream);
                    }

                    if (utilizador.Foto != null)
                    {
                        string localFoto = utilizador.Foto;
                        string ficheiroFoto = Directory.GetCurrentDirectory() + "/wwwroot" +localFoto;
                        if (System.IO.File.Exists(ficheiroFoto))
                            System.IO.File.Delete(ficheiroFoto);
                    }
                    
                    utilizador.Foto = nomefoto;
                }

                _context.Update(utilizador);
                await _context.SaveChangesAsync();
            }

            await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);


            
            
            if (Input.PhoneNumber != utilizador.Telefone)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao tentar alterar o número de telefone.";
                    return RedirectToPage();
                }
            }


            StatusMessage = "O seu perfil foi atualizado";
            return RedirectToPage();
        }
    }
}
