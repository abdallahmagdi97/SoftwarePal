using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } // (foreign key referencing the Product model)
        public int Views { get; set; }
        public int Sales { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Shares { get; set; }
    }
}
