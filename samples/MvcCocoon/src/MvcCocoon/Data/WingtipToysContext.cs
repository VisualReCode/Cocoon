using Microsoft.EntityFrameworkCore;

namespace MvcCocoon.Data
{
    public class WingtipToysContext : DbContext
    {
        public WingtipToysContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<CartItem> CartItems { get; set; }
        
        public DbSet<Product> Products { get; set; }
    }
}