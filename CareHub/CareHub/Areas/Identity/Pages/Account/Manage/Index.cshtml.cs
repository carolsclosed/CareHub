// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CareHub.Data;
using CareHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CareHub.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        
        [DisplayName("Nome")]
        public string Username { get; set; }

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
            [Display(Name = "Número de telefone")]
            public string PhoneNumber { get; set; }

            [DisplayName("Nome")]
            public string Nome { get; set; }
            
            [DisplayName("Região")]
            public string Regiao { get; set; }
            
            public string Foto { get; set; }
            
            
            public IFormFile FotoFicheiro { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);
            
            Input = new InputModel
            {
                PhoneNumber = utilizador?.Telefone,
                Regiao = utilizador?.Regiao,
                Foto = utilizador?.Foto,
                Nome = utilizador?.Nome
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.IdentityUserName == User.Identity.Name);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var numeroTelefone = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != numeroTelefone)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                utilizador.Telefone = Input.PhoneNumber;
            }
            

            if (Input.FotoFicheiro.ContentType == "image/png" || Input.FotoFicheiro.ContentType == "image/jpeg")
            {
                var FotoExistente = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + utilizador.Foto);
                var FotosCaminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImagensUtilizadores/");
                if (!Directory.Exists(FotosCaminho))
                {
                    Directory.CreateDirectory(FotosCaminho);
                }
                var FotoNome = Guid.NewGuid().ToString() + Path.GetExtension(Input.FotoFicheiro.FileName);
                var FotoCaminho = Path.Combine("/ImagensUtilizadores/"+FotoNome);
                
                using(var fileStream = new FileStream(FotosCaminho+FotoNome, FileMode.Create))
                {
                    await Input.FotoFicheiro.CopyToAsync(fileStream);
                }

                if (System.IO.File.Exists(FotoExistente))
                {
                    string NomeFoto = System.IO.Path.GetFileName(FotoExistente);
                    if (!NomeFoto.Equals("user.jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        System.IO.File.Delete(FotoExistente);
                    }
                }
                
                utilizador.Foto = FotoCaminho;
            }
            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
