using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}