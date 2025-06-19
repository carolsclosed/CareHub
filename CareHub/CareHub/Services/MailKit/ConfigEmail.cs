namespace CareHub.Services.MailKit;

public class ConfigEmail
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool UseSsl { get; set; }
    public bool UseStartTls { get; set; }

}

