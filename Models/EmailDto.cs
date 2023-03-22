using System.ComponentModel.DataAnnotations;

namespace MyLoginApi.Models
{
    public class EmailDto
    {
        [Key]
        public string To { get; set; } = string.Empty;
        
        public string Subject { get; set; } =string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
