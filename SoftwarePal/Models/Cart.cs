using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
