// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CareHub.Data;
using CareHub.Models;
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CareHub.Areas.Identity.Pages.Account
{
    
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailer _emailSender;
        private readonly ApplicationDbContext _context;
        
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IMailer emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-passe")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar palavra-passe")]
            [Compare("Password", ErrorMessage = "A palavra-passe e a confirmação não coincidem.")]
            public string ConfirmPassword { get; set; }
            
            public Utilizadores Utilizador { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            // Read the JSON file
            var jsonContent = System.IO.File.ReadAllText("./wwwroot/regioes.json");
            var regioes = JsonSerializer.Deserialize<List<InfoRegiao>>(jsonContent);

            // Ensure regioes is not null before proceeding
            if (regioes != null)
            {
                // Filter the regions based on some condition, if necessary
                regioes = regioes.Where(r =>
                    r.Provincia != null && r.Distritos != null && r.Regioes != null && r.Nome != null
                ).ToList();
        
                // Create a dropdown list with unique values (name, district, province, region)
                var regioesDropdown = regioes
                    .SelectMany(r => new[] { r.Nome, r.Distritos, r.Provincia, r.Regioes })
                    .Where(x => !string.IsNullOrWhiteSpace(x)) // Ensure no null or empty entries
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                // Assign values to ViewBag
                ViewData["Regioes"] = regioesDropdown ?? new List<string>();
            }
            else
            {
                ViewData["Regioes"] = new List<string>(); // Default empty list if null
            }


            ReturnUrl = returnUrl;

            // Initialize external logins
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var emailExists = await _userManager.FindByEmailAsync(Input.Email);
                var nomeExists = await _userManager.FindByNameAsync(Input.Utilizador.Nome);
                if (emailExists != null)
                {
                    ModelState.AddModelError("Input.Email", "Já existe uma conta com este email.");
                    return Page(); // Reexibe o formulário com erro
                }

                if (nomeExists != null)
                {
                    ModelState.AddModelError("Input.Utilizador.Nome","Já existe alguem com este nome");
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilizador criou uma nova conta com palavra-passe.");
                    
                    //cria o utilizador na tabela Utilizadores
                    Input.Utilizador.IdentityUserName = user.UserName;
                    Input.Utilizador.Foto = "/ImagensUtilizadores/user.jpg";
                    _context.Add(Input.Utilizador);
                    
                    _context.SaveChanges();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email",
                        $"Por favor confirme a sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
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
