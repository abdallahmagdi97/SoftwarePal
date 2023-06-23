using SoftwarePal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SoftwarePal.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<License> Licenses { get; set; } = null!;
        public DbSet<SubItem> SubItems { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;    
        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Voucher> Vouchers { get; set; } = null!;
        public DbSet<AboutUs> AboutUs { get; set; } = null!;
        public DbSet<IncludedSubItem> IncludedSubItems { get; set; } = null!;
        public DbSet<ItemPriceRule> ItemPriceRules { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<ItemImage> ItemImages { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<ItemSubItems> ItemSubItems { get; set; } = null!;
        public DbSet<WishList> WishLists { get; set; } = null!;
        public DbSet<RelatedProducts> RelatedProducts { get; set; } = null!;
        public DbSet<ContactUs> ContactUs { get; set; } = null!;
        public DbSet<Statistics> Statistics { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Search> Search { get; set; } = null!;

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
