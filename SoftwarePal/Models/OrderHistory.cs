using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class OrderHistory
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } // (foreign key referencing the Product model)
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; } = string.Empty; // (foreign key referencing the User model, if applicable)
    }
}
