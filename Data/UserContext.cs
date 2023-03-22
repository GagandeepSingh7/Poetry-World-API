using Microsoft.EntityFrameworkCore;
using MyLoginApi.Models;

namespace MyLoginApi.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext>options) :base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> clients { get; set; }

        public DbSet<UserRegisterRequest> userregister { get; set; }

        public DbSet<UserLoginRequest>  LoginRequests { get; set; }

        public DbSet<EmailDto> dto { get; set; }

        public DbSet<ResetPasswordRequest>  request { get; set; }

        
    }
}
