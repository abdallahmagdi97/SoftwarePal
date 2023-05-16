using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; }
    }
}
