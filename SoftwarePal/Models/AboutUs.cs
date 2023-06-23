using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class AboutUs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; } = string.Empty;
        [Required]
        public string? Description { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string? UserCreated { get; set; } = string.Empty;
        public string? UserUpdated { get; set; } = string.Empty;
    }
}
