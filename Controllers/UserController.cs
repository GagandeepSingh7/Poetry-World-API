
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLoginApi.Data;
using MyLoginApi.Models;
using MyLoginApi.Services.EmailService;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;

namespace VerifyEmailForgotPassword.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext context;
        private readonly IEmailService emailService;

        public UserController(UserContext context, IEmailService emailService)
        {
            this.context = context;
            this.emailService = emailService;
        }


        [HttpGet("details")]

        public async Task<IEnumerable<Client>> GetUser() => await context.clients.ToListAsync();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (context.clients.Any(u => u.Username==request.Username))
            {
                return BadRequest("User Already Exists");
            }
            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new Client
            { 
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken= Guid.NewGuid().ToString()

            };
            var user1 = new ResetPasswordRequest
            {
                
                Token="",
                Password = request.Password,
                ConfirmPassword = request.Password


            };
            context.request.Add(user1);
            context.clients.Add(user);
            await context.SaveChangesAsync();
            return Ok(user);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        private void CreateTokenHash(string Token, out byte[] TokenHash, out byte[] TokenSalt)
        {
            Token = Guid.NewGuid().ToString();
            
            using (var hmac = new HMACSHA512())
            {
                TokenSalt = hmac.Key;
                TokenHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Token));

            }
        }
        
    private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private bool VerifyPasswordHash(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =  hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            
            var user = await context.clients.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }

            //if (user.VerifiedAt == null)
            //{
            //    return BadRequest("Not verified!");
            //}

            return Ok($"Welcome back, {user.Username}! :)");
        }


        // [HttpPost("verify")]
        // public async Task<IActionResult> Verify(string token)
        // {
        //     var user = await context.Clients.FirstOrDefaultAsync(u => u.VerificationToken == token);
        //     if (user == null)
        //     {
        //         return BadRequest("InValid ");
        //     }
        //
        //     user.VerifiedAt = DateTime.Now;
        //     await context.SaveChangesAsync();
        //     return Ok("User Verified");
        //
        // }
        //
        //

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword( EmailDto request)
        {
            var user = await context.clients.FirstOrDefaultAsync(u => u.Username == request.To);
            if (user == null)
            {
                return BadRequest("InValid ");
            }
            
            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            emailService.SendEmail(request);
            await context.SaveChangesAsync();
            return Ok("You may now reset your password");

        }
        
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            
            var user = await context.clients.FirstOrDefaultAsync(u => u.VerificationToken == request.Token);
            if (user == null)
            {
                return BadRequest("InValid Token ");
            }
            
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash=passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ResetTokenExpires = null;
            user.ResetTokenExpires = null;
           
            await context.SaveChangesAsync();
            return Ok("Password Successfully Reset !");

        }

        [HttpDelete]

        public async Task<IActionResult> RemoveUser(string Username)
        {
            var user = await context.clients.FirstOrDefaultAsync(o=>o.Username==Username);
            if (user == null)   
            {
                return BadRequest("Invalid Request");
            }

            context.clients.Remove(user);
            await context.SaveChangesAsync();
            return Ok();

        }

    }
}
