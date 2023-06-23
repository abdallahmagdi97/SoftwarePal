using System;
using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Code { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; } = string.Empty;
        public string? UserUpdated { get; set; } = string.Empty;
    }
}
