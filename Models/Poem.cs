using System.ComponentModel.DataAnnotations;

namespace MyLoginApi.Models
{
    public class Poem
    {
        [Key]
        public int Id { get; set; }
        public string PoemTitle { get; set; }

        public string PoemDescription { get; set; } = string.Empty;

    }
}
