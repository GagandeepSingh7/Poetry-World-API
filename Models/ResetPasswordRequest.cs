using System.ComponentModel.DataAnnotations;

namespace MyLoginApi.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        [Key]
        public string Token { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
