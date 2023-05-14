using System;

namespace SoftwarePal.Models
{
    public class Voucher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal Amount { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Status { get; set; }
    }
}
