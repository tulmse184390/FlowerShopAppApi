using FlowerShopApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Infrastructure
{
    public class FlowerShopAppContext : DbContext
    {
        public FlowerShopAppContext(DbContextOptions<FlowerShopAppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.UserId);

                e.Property(x => x.UserName).HasMaxLength(50).IsRequired();
                e.HasIndex(x => x.UserName).IsUnique();

                e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
                e.Property(x => x.Email).HasMaxLength(100);
                e.Property(x => x.PhoneNumber).HasMaxLength(15);
                e.Property(x => x.Address).HasMaxLength(255);

                e.Property(x => x.Role).HasMaxLength(20).HasDefaultValue("USER"); 
                e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP"); 
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("categories");
                e.HasKey(x => x.CategoryId);

                e.Property(x => x.CategoryName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(x => x.ProductId);

                e.Property(x => x.ProductName).HasMaxLength(100).IsRequired();
                e.Property(x => x.BriefDescription).HasMaxLength(255);

                e.Property(x => x.Price).HasPrecision(18, 2);
                e.Property(x => x.StockQuantity).HasDefaultValue(0);

                e.Property(x => x.IsDeleted).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<ProductImage>(e =>
            {
                e.ToTable("product_images");
                e.HasKey(x => x.ImageId);

                e.Property(x => x.ImageUrl).HasMaxLength(255).IsRequired();
                e.Property(x => x.IsPrimary).HasDefaultValue(false);

                e.HasOne(i => i.Product)
                 .WithMany(p => p.ProductImages) 
                 .HasForeignKey(i => i.ProductId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cart>(e =>
            {
                e.ToTable("carts");
                e.HasKey(x => x.CartId);

                e.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("ACTIVE");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(c => c.User)
                 .WithMany(u => u.Carts)
                 .HasForeignKey(c => c.UserId);
            });

            modelBuilder.Entity<CartItem>(e =>
            {
                e.ToTable("cart_items");
                e.HasKey(x => x.CartItemId);

                e.Property(x => x.Price).HasPrecision(18, 2);

                e.HasOne(ci => ci.Cart)
                 .WithMany(c => c.CartItems)
                 .HasForeignKey(ci => ci.CartId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ci => ci.Product)
                 .WithMany(p => p.CartItems)
                 .HasForeignKey(ci => ci.ProductId);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("orders");
                e.HasKey(x => x.OrderId);

                e.Property(x => x.TotalAmount).HasPrecision(18, 2);
                e.Property(x => x.PaymentMethod).HasMaxLength(50);
                e.Property(x => x.ShippingAddress).HasMaxLength(255);
                e.Property(x => x.OrderStatus).HasMaxLength(50);
                e.Property(x => x.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(o => o.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.UserId);
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.ToTable("order_items");
                e.HasKey(x => x.OrderItemId);

                e.Property(x => x.ProductName).HasMaxLength(100);
                e.Property(x => x.Price).HasPrecision(18, 2);

                e.HasOne(oi => oi.Order)
                 .WithMany(o => o.OrderItems)
                 .HasForeignKey(oi => oi.OrderId)
                 .OnDelete(DeleteBehavior.Cascade); 

                e.HasOne(oi => oi.Product)
                 .WithMany(p => p.OrderItems)
                 .HasForeignKey(oi => oi.ProductId);
            });

            modelBuilder.Entity<Payment>(e =>
            {
                e.ToTable("payments");
                e.HasKey(x => x.PaymentId);

                e.Property(x => x.Amount).HasPrecision(18, 2);
                e.Property(x => x.PaymentGateway).HasMaxLength(50);
                e.Property(x => x.PaymentStatus).HasMaxLength(50);
                e.Property(x => x.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(p => p.Order)
                 .WithMany(o => o.Payments)
                 .HasForeignKey(p => p.OrderId);
            });

            modelBuilder.Entity<Notification>(e =>
            {
                e.ToTable("notifications");
                e.HasKey(x => x.NotificationId);

                e.Property(x => x.Message).HasMaxLength(255);
                e.Property(x => x.IsRead).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(n => n.User)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId);
            });

            modelBuilder.Entity<ChatRoom>(e =>
            {
                e.ToTable("chat_rooms");
                e.HasKey(x => x.RoomId);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(c => c.User)
                 .WithMany(u => u.ChatRooms)
                 .HasForeignKey(c => c.UserId);
            });

            modelBuilder.Entity<ChatMessage>(e =>
            {
                e.ToTable("chat_messages");
                e.HasKey(x => x.MessageId); 

                e.Property(x => x.SenderRole).HasMaxLength(20);
                e.Property(x => x.SentAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                e.HasOne(m => m.ChatRoom)
                 .WithMany(r => r.ChatMessages)
                 .HasForeignKey(m => m.RoomId);
            });

            modelBuilder.Entity<StoreLocation>(e =>
            {
                e.ToTable("store_locations");
                e.HasKey(x => x.LocationId);

                e.Property(x => x.StoreName).HasMaxLength(100);
                e.Property(x => x.Address).HasMaxLength(255);

                e.Property(x => x.Latitude).HasPrecision(9, 6);
                e.Property(x => x.Longitude).HasPrecision(9, 6);
            });
        }
    }
}
