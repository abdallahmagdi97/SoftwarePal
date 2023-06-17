using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        [NotMapped]
        public Item? ItemDetails { get; set; }
        public int Qty { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UserCreated { get; set; } = string.Empty;
    }
}