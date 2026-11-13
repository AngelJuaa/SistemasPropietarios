using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Beer> Beers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<backend.Models.Inventory> Inventories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>()
            .HasIndex(b => b.Name)
            .IsUnique();

        modelBuilder.Entity<Beer>()
            .HasOne(b => b.Brand)
            .WithMany()
            .HasForeignKey(b => b.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Brand>()
            .HasOne(b => b.Supplier)
            .WithMany(s => s.Brands)
            .HasForeignKey(b => b.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Inventory)
            .WithMany()
            .HasForeignKey(s => s.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}