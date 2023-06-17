using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; } = string.Empty;
        [Required]
        public string? Content { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; } = string.Empty;
        public string? Author { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; } = string.Empty;
        public string? UserUpdated { get; set; } = string.Empty;
    }
}
