using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
    }
}
