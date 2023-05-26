using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class AboutUs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
