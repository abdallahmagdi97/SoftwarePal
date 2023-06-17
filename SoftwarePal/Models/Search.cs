using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Search
    {
        [Key]
        public int Id { get; set; }
        public string Query { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
