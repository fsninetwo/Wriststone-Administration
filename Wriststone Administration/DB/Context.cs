using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using Wriststone.Models.Table;

namespace Wriststone_Administration.DB
{
    class Context : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(@"host=localhost;database=wriststone;user id=postgres;password=CfrVfqVjkc1Nfqvc;");
        }
    }
}
