using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class ItemPriceRule
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int MinQty { get; set; } = 1;
        public int? MaxQty { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
