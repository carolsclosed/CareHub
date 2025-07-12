using System.Text.Json; // Importa o namespace System.Text.Json para funcionalidade de serialização e desserialização JSON.
using System.Text.Json.Serialization;
using CareHub.Data; // Importa o namespace CareHub.Data
using CareHub.Models; // Importa o namespace CareHub.Models
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Authorization; // Importa o namespace Microsoft.AspNetCore.Authorization, usado para controlo de acesso e autorização.
using Microsoft.AspNetCore.Mvc; // Importa o namespace Microsoft.AspNetCore.Mvc, que contém classes e interfaces para construir aplicações web MVC no ASP.NET Core.
using Microsoft.AspNetCore.Routing.Constraints; // Importa o namespace System.Text.Json.Serialization para atributos de serialização JSON.
using Microsoft.EntityFrameworkCore; 

namespace CareHub.Controllers 
{
    /// <summary>
    /// controller para a candidatura a médico
    /// </summary>
    [Authorize] 
    public class FormMedicoController : Controller 
    {
        private readonly ApplicationDbContext _context; 
        private readonly IMailer _mailer;
        
       
        public FormMedicoController(ApplicationDbContext context, IMailer mailer)
        {
            _context = context;
            _mailer = mailer;
        }

        // GET: Formulário
        /// <summary>
        /// Método para candidatura apenas possivel para utilizadores autenticados
        /// </summary>
        /// <returns></returns>
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
                
                ViewBag.MensagemTitulo = "A aguardar resposta";
                ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
               
                return View("Aviso");
            }

            
            var vm = new FormMedicoViewModel
            {
               
                Doutor = new Doutores { IdUtil = utilizador.IdUtil },
               
                Regioes = CarregarRegioes()
            };

           
            return View(vm);
        }
        
        /// <summary>
        /// método para carregar as regiôes
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Método para candidatura para doutores, apenas utilizadores autenticados
        /// podem se candidatar, é enviado um email para o candidato a confirmar a sua candidatura 
        /// </summary>
        /// <param name="doutor"></param>
        /// <returns></returns>
        // POST: Submete o formulário do doutor
        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult>  FormMedico(Doutores doutor) 
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
                
                
                var emailCorpo = $"Olá {doutor.Nome},<br><br>Obrigado por submeter formulário. Assim que possível, contactaremos você. <br><br>Detalhes: <br>Porquê da candidatura: : {doutor.Descricao}<br>Região: {doutor.DistritoProfissional}<br>Especialidade: {doutor.Especialidade}<br>Número cédula: {doutor.nCedula} <br><br>Se houver algo de errado na informação disposta não exite em contactar-nos <br><br>Atenciosamente,<br>Equipa CareHub";
                var emailAssunto = "Confirmação do formulário";
                
                
                try
                {
                
                    
                    await _mailer.SendEmailAsync(doutor.email, emailAssunto, emailCorpo);
                    
                    ViewBag.MensagemTitulo = "O seu registo como doutor foi submetido com sucesso!";
                    ViewBag.MensagemCorpo = "Agradecemos o seu interesse na CareHub.";
                   
                    emailCorpo = $"Um doutor candidatou-se<br><br>Detalhes<br><br>Nome: {doutor.Nome}<br>Região: {doutor.DistritoProfissional}<br>Especialidade: {doutor.Especialidade}<br>Email: {doutor.email}<br>Porquê da candidatura: {doutor.Descricao}<br><br>";
                    var emailCarehub = "carehubprofessionals@gmail.com";
                    emailAssunto = "Nova candidatura";
                    
                    await _mailer.SendEmailAsync(emailCarehub, emailAssunto, emailCorpo);
                    _context.Add(doutor); 
                    await _context.SaveChangesAsync();
                
                }
                catch (Exception ex)
                {
                    ViewBag.MensagemCorpo =
                        "Ocurreu um erro na submissão por favor tente novamente mais tarde";
                }
                

                
                
                return View("Aviso");
            }

            
            
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