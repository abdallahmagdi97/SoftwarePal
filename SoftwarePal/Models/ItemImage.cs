using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class ItemImage
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public int ImageOrder { get; set; }
    }
}
