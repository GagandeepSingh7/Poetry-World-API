using MyLoginApi.Models;

namespace MyLoginApi.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
