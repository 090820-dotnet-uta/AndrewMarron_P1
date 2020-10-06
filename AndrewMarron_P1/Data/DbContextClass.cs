using Microsoft.EntityFrameworkCore;
using RevatureP1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1
{
    public class DbContextClass : DbContext
    {
        /// <summary>
        /// This is a class implementing DbContext, which it uses to connect to the database via EntityFrameworkCore
        /// </summary>
        public DbContextClass()
        { }

        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {

        }
        // Independent tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        // Derived tables
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BundleRelation> BundleRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(p => new { p.UserName })
                .IsUnique(true);
            modelBuilder.Entity<StockItem>() // Same location cannot list two of same product
                .HasAlternateKey(c => new { c.LocationId, c.ProductId });
        }
    }
}
