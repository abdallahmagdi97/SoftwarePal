using System.ComponentModel.DataAnnotations;

namespace SoftwarePal.Models
{
    public class UserVoucher
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
    }
}
