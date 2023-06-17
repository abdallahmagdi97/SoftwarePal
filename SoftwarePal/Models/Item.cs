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
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool IsFeatured { get; set; } = false;
        public string OperatingSystem { get; set; } = string.Empty;
        public int NumberOfUsers { get; set; } = 1;
        public string Specs { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserCreated { get; set; } = string.Empty;
        public string? UserUpdated { get; set; }
        [NotMapped]
        public List<ItemImage>? ItemImages { get; set; }
        [NotMapped]
        public List<ItemPriceRule>? ItemPriceRules { get; set; }
        [NotMapped]
        public List<int>? SubItemsIds { get; set; }
        [NotMapped]
        public List<SubItem>? IncludedSubItems { get; set; }
        public string Slug { get; set; } = string.Empty;
    }
}
