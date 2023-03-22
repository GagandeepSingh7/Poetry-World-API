using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyLoginApi.Data;
using MyLoginApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace MyLoginApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserContext context;
        private readonly IConfiguration configuration;

        public LoginController(UserContext context , IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }



        [HttpGet]
        
        public async Task<IEnumerable<User>> GetUser() => await context.Users.ToListAsync();






        [HttpPost("Register")]

        public async Task<IActionResult> Registernormal ([FromBody] User user)
        {
            if (context.Users.Any(m => m.Username == user.Username))
            {
                return BadRequest("UserAlreadyExists");
            }
            string hash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var newuser = new User
            {
                Username = user.Username,
                Password = hash
            };
            context.Add(user);
            await context.SaveChangesAsync();
            
            if(user==null)
            {
                return BadRequest();
            }
            return Ok("User Created Successfully");
        }





        [HttpPost("Validate")]
        //For Role Based Authorization
        //[Authorize (Roles = "Admin")]
        public async Task<IActionResult> Validatenormal([FromBody] User users)
        {
            var userdetail = context.Users.FirstOrDefault(u => u.Username == users.Username && u.Password == users.Password);
            string token = CreateToken(users);
            return Ok(token);
        }





        private string CreateToken(User user)
        {
            List<Claim> claims = new()

            {

                new Claim(ClaimTypes.Name, user.Username),

                new Claim(ClaimTypes.Role, "Admin")

            };

 

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(

                configuration.GetSection("AppSettings:Token").Value));

 

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

 

            var token = new JwtSecurityToken(

                claims: claims,

                expires: DateTime.Now.AddDays(1),

                signingCredentials: creds);

 

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

 

            return jwt;
        }

    }
}
