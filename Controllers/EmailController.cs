using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLoginApi.Models;
using MyLoginApi.Services.EmailService;

namespace MyLoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            emailService.SendEmail(request);
            return Ok();
        }
    }
}
