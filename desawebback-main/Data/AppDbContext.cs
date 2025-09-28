using Microsoft.EntityFrameworkCore;
using MessageApi.Models;
using HelloApi.Models;

namespace MessageApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Detail> Details => Set<Detail>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
        .HasMany(t => t.Invoices)
        .WithOne(t => t.Client)
        .HasForeignKey(t => t.ClientId);

        modelBuilder.Entity<Invoice>()
        .HasMany(t => t.Details)
        .WithOne(t => t.Invoice)
        .HasForeignKey(t => t.InvoiceId);

        modelBuilder.Entity<Product>()
        .HasMany(t => t.Details)
        .WithOne(t => t.Product)
        .HasForeignKey(t => t.ProductId);

        modelBuilder.Entity<Order>()
        .HasMany(t => t.OrderDetails)
        .WithOne(t => t.Order)
        .HasForeignKey(t => t.OrderId);

        modelBuilder.Entity<Item>()
        .HasMany(t => t.OrderDetails)
        .WithOne(t => t.Item)
        .HasForeignKey(t => t.ItemId);

        modelBuilder.Entity<Person>()
        .HasMany(t => t.Orders)
        .WithOne(t => t.Person)
        .HasForeignKey(t => t.PersonId);

        modelBuilder.Entity<Role>()
        .HasMany(t => t.Users)
        .WithOne(t => t.Role)
        .HasForeignKey(t => t.RoleId);

        //seed tables to login        
        /*modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin@miumg.edu.gt", Password = BCrypt.Net.BCrypt.HashPassword("Admin123$#2025"), RoleId = 1 },
            new User { Id = 2, Username = "user@miumg.edu.gt", Password = BCrypt.Net.BCrypt.HashPassword("User123$#2025"), RoleId = 2 }
        );*/

    }
    
}
