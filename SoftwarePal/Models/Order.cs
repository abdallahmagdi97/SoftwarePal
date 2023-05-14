namespace SoftwarePal.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int LicenseId { get; set; }
        public decimal Price { get; set; }
    }
}
