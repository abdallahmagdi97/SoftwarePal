using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class RelatedProducts
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } // (foreign key referencing the Product model)
        public int RelatedProductId { get; set; } // (foreign key referencing the Product model)
    }
}
