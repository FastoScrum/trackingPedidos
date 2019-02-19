using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TrackingPedidos.Services
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            Options.SMTP_Username = "andres.andy63@hotmail.com";
            Options.SMTP_Password = "amoaliga07";
            Options.SMTP_Server = "smtp.live.com";
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SMTP_Password, subject, message, email);
        }

        public Task Execute(string password, string subject, string message, string email)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Options.SMTP_Username);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Subject = subject;

            SmtpClient client = new SmtpClient(Options.SMTP_Server);

            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(Options.SMTP_Username, password);

            return client.SendMailAsync(mailMessage);
        }
    }
}