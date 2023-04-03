using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MyLoginApi.Data;
using MyLoginApi.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace MyLoginApi.Services.EmailService
{
    public class EmailService :  IEmailService
    {
        private readonly IConfiguration configuration;
        private readonly UserContext context;

        public EmailService(IConfiguration configuration, UserContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public void SendEmail(EmailDto request)
        {
            
                string token = context.clients.First(x => x.Username == request.To).VerificationToken;

                string resetUrl = $"https://localhost:7088/ResetPassword/ResetPassword?token={token}";
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Poetica-Reset your Password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = $"Click this link to reset your password {resetUrl} and you will be redirected to Reset Password Page" };
            using var smtp = new SmtpClient();
            smtp.Connect(configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration.GetSection("EmailUsername").Value, (configuration.GetSection("EmailPassword").Value));
            smtp.Send(email);
            smtp.Disconnect(true);
            

        }

        private string TextPart(TextFormat plain)
        {
            throw new NotImplementedException();
        }
    }
}
