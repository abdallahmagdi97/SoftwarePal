﻿namespace SoftwarePal.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public string? Author { get; set; }
        public string? Url { get; set; }
        public bool Status { get; set; }
    }
}
