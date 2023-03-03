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
        
    }
}
