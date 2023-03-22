using System.ComponentModel.DataAnnotations;

namespace MyLoginApi.Models
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress, Key]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }


    }
}
