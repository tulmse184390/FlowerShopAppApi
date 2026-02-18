using FlowerShopApp.Domain.Entities;

namespace FlowerShopApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductImage> ProductImages { get; } 
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Cart> Carts { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Payment> Payments { get; }   
        IGenericRepository<Notification> Notifications { get; } 
        IGenericRepository<ChatRoom> ChatRooms { get; } 
        IGenericRepository<ChatMessage> ChatMessages { get; }   
        IGenericRepository<StoreLocation> StoreLocations { get; }   

        Task<int> CompleteAsync();
    }
}
