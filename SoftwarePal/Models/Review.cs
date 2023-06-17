using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int? ProductId { get; set; } // (foreign key referencing the Product model)
        public string ReviewText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Pros { get; set; } = string.Empty;
        public string Cons { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
