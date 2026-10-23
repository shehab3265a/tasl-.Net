using Microsoft.EntityFrameworkCore;
using tasl_Net.Models;

namespace tasl_Net.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
    new Category
    {
        Id = 1,
        Name = "Electronics"
    },
    new Category
    {
        Id = 2,
        Name = "Laptops"
    }
);

modelBuilder.Entity<Product>().HasData(
    new Product
    {
        Id = 1,
        Name = "Laptop",
        Price = 25000,
        CategoryId = 1,
        IsDeleted = false
    },
    new Product
    {
        Id = 2,
        Name = "Phone",
        Price = 15000,
        CategoryId = 1,
        IsDeleted = false
    },
    new Product
    {
        Id = 3,
        Name = "MacBook",
        Price = 50000,
        CategoryId = 2,
        IsDeleted = false
    }
);

        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name);

        modelBuilder.Entity<Product>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}