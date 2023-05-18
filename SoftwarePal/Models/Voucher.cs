using System;
using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Code { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Status { get; set; }
    }
}
