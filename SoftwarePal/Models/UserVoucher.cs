using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class UserVoucher
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UserCreated { get; set; }
        public string? UserUpdated { get; set; }
    }
}
