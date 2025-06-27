using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using CareHub.Services;
using MailKit.Security;

namespace CareHub.Services.MailKit
{
    public interface IMailer
    {
        Task SendEmailAsync(string email, string subject, string body);
    }

    public class Mailer : IMailer
    {
        private readonly ConfigEmail _smtpSettings;
        private readonly IWebHostEnvironment _env;

        public Mailer(IOptions<ConfigEmail> smtpSettings, IWebHostEnvironment env)
        {
            _smtpSettings = smtpSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email address is required", nameof(email));
            }

            if (!IsValidEmail(email))
            {
                throw new ArgumentException("Invalid email format", nameof(email));
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress(email, email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using var client = new SmtpClient();
                // Only disable certificate validation in Development
                if (_env.IsDevelopment())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                var socketOptions = SecureSocketOptions.Auto;

                if (_smtpSettings.Port == 465)
                    socketOptions = SecureSocketOptions.SslOnConnect;
                else if (_smtpSettings.Port == 587)
                    socketOptions = SecureSocketOptions.StartTls;

                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, socketOptions);


                // Only authenticate if credentials are provided
                if (!string.IsNullOrEmpty(_smtpSettings.UserName) && !string.IsNullOrEmpty(_smtpSettings.Password))
                {
                    await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (SmtpCommandException ex)
            {
                throw new InvalidOperationException($"SMTP error: {ex.Message}", ex);
            }
            catch (SmtpProtocolException ex)
            {
                throw new InvalidOperationException($"SMTP protocol error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}