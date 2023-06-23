using System.ComponentModel.DataAnnotations;
using System;

namespace SoftwarePal.Models
{
    public class License
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? Key { get; set; } = string.Empty;
        public string? UserPurchased { get; set; } = string.Empty;
        public bool IsPurchased { get; set; } = false;
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
