using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
    }
}
