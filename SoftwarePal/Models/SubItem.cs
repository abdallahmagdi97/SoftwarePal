using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class SubItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [NotMapped]
        public IFormFile? Image { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; }
    }
}
