using System.ComponentModel.DataAnnotations;
namespace MyLoginApi.Models
{
    public class Client
    {
        [Key]
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;   
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public byte[] TokenHash { get; set; } = new byte[32];
        public byte[] TokenSalt { get; set; } = new byte[32];
        public string VerificationToken { get; set; }
        public string? PasswordResetToken { get; set; } = string.Empty;
        
        public DateTime? VerifiedAt { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        

        
    }
}
