using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
            
        }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Stock>().HasData(new List<Stock>
            {
                new Stock { Id = 1 , ProductId  = 2 , Count = 5},
                new Stock { Id = 2 , ProductId  = 2 , Count = 5}
            });
        }
    }
}
