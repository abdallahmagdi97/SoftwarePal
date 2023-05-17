namespace SoftwarePal.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Completed,
        Returned
    }
}
