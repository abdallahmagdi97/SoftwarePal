using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwarePal.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
    }
}
