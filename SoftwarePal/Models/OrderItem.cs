using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int LicenseId { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
