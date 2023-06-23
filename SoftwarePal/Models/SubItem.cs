using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class SubItem
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? Image { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; } = string.Empty;
        public string? UserUpdated { get; set; } = string.Empty;
    }
}
