using System.Threading.Tasks;
using CareHub.Services.MailKit;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CareHub.Services.MailKit
{
    public class EmailSender : IEmailSender
    {
        private readonly IMailer _mailer;

        public EmailSender(IMailer mailer)
        {
            _mailer = mailer;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await _mailer.SendEmailAsync(email, subject, htmlMessage);
        }
    }
}