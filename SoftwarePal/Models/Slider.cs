using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; }
        public string? SubTitle { get; set; }
        public string? ButtonText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
