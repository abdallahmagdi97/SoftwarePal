using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public bool IsFeatured { get; set; } = false;
        public string? OperatingSystem { get; set; }
        public int NumberOfUsers { get; set; } = 1;
        public string? Specs { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        [NotMapped]
        public List<ItemImage> ItemImages { get; set; }
        [NotMapped]
        public List<ItemPriceRule> ItemPriceRules { get; set; }
        [NotMapped]
        public List<IncludedSubItem> IncludedSubItems { get; set; }
    }
}
