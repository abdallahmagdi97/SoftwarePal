using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SoftwarePal.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<SubItem> SubItems { get;  set; }
        public DbSet<Item> Items { get;  set; }
        public DbSet<Category> Categories { get;  set; }
        public DbSet<Blog> Blogs { get;  set; }
        public DbSet<Cart> Carts { get;  set; }
        public DbSet<CartItem> CartItems { get;  set; }
        public DbSet<Order> Orders { get;  set; }
        public DbSet<Slider> Sliders { get;  set; }
        public DbSet<Voucher> Vouchers { get;  set; }
        public DbSet<AboutUs> AboutUs { get;  set; }
        public DbSet<IncludedSubItem> IncludedSubItems { get;  set; }
        public DbSet<ItemPriceRule> ItemPriceRules { get;  set; }
        public DbSet<Team> Teams { get;  set; }
        public DbSet<ItemImage> ItemImages { get;  set; }
        public DbSet<OrderItem> OrderItems { get;  set; }
        public DbSet<ItemSubItems> ItemSubItems { get;  set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies(); // Enables lazy loading
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<License>().HasKey(l => l.Id);
            
        }
    }
}
