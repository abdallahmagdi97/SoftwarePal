namespace SoftwarePal.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
