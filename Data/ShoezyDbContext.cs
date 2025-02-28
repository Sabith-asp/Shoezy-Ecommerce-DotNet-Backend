using Microsoft.EntityFrameworkCore;
using Shoezy.Models;
using Shoezy.Repositories;

namespace Shoezy.Data
{
    public class ShoezyDbContext : DbContext
    {
        public ShoezyDbContext(DbContextOptions<ShoezyDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }

        public DbSet<Cart> cart { get; set; }
        public DbSet<CartItem> cartitem { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
    new Category { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "Sneakers" },
    new Category { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = "Running" },
    new Category { Id = new Guid("33333333-3333-3333-3333-333333333333"), Name = "Boots" },
    new Category { Id = new Guid("44444444-4444-4444-4444-444444444444"), Name = "Sandals" },
    new Category { Id = new Guid("55555555-5555-5555-5555-555555555555"), Name = "Formal" },
    new Category { Id = new Guid("66666666-6666-6666-6666-666666666666"), Name = "Sports" }

);
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.user)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.products)
                .WithMany()
                .HasForeignKey(w => w.ProductId);


            modelBuilder.Entity<Cart>()
                .HasMany(c => c.cartItem)
                .WithOne(ci => ci.cart)
                .HasForeignKey(ci => ci.CartId);


            modelBuilder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany()
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.cart)
                .WithOne(c => c.users)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); 


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c=>c.Products)
                .HasForeignKey(p=>p.CategoryId);
        }

    }
}
