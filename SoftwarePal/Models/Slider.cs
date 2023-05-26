using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Url { get; set; }
        [Required]
        [NotMapped]
        public IFormFile? Image { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
