using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class ItemImage
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? ImageName { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public int ImageOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
