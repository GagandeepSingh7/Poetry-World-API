using System.ComponentModel.DataAnnotations;

namespace MyLoginApi.Models
{
    public class UserLoginRequest
    {
        [Key]
        [Required, EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
