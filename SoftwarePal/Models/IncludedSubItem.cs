using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class IncludedSubItem
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int SubItemId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
