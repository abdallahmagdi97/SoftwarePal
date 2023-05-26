using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public bool InMenu { get; set; } = true;
        public int Order { get; set; }
        public bool Status { get; set; } = true;
        public string? ImageName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
