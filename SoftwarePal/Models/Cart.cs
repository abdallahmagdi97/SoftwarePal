using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [NotMapped]
        public List<CartItem>? CartItems { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class CartRequest
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}
