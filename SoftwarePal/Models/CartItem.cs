﻿namespace SoftwarePal.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }
}