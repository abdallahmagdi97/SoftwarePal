using System.ComponentModel.DataAnnotations;
using System;

namespace SoftwarePal.Models
{
    public class License
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Key { get; set; }
        public bool IsPurchased { get; set; } = false;
        public bool Status { get; set; } = true;
    }
}
