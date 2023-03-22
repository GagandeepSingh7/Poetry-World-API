using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLoginApi.Data;
using MyLoginApi.Models;
using MyLoginApi.Services.EmailService;

namespace MyLoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly UserContext context;

        public EmailController(IEmailService emailService, UserContext context)
        {
            this.emailService = emailService;
            this.context = context;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            if(context.Users.Any(c => c.Username == request.To))
            {
                emailService.SendEmail(request);
                return Ok();
            }
            else
            {
                return BadRequest("Invalid Email Address");
            }
            
            
        }

    }
}
