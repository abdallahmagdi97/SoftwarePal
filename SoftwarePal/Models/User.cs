using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SoftwarePal.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }
        public UserRole UserRole { get; set; }
        public bool Status { get; set; } = true;
    }

    public enum UserRole
    {
        Admin,
        User
    }
}
