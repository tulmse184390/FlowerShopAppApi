using FlowerShopApp.Domain.Entities;
using FlowerShopApp.Domain.Interfaces;

namespace FlowerShopApp.Infrastructure.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlowerShopAppContext _context;

        private IGenericRepository<User>? _users;
        private IGenericRepository<Product>? _products;
        private IGenericRepository<ProductImage>? _productImages;
        private IGenericRepository<Category>? _categories;
        private IGenericRepository<Cart>? _carts;
        private IGenericRepository<CartItem>? _cartItems;
        private IGenericRepository<Order>? _orders;
        private IGenericRepository<OrderItem>? _orderItems;
        private IGenericRepository<Payment>? _payments;
        private IGenericRepository<Notification>? _notifications;
        private IGenericRepository<ChatRoom>? _chatRooms;
        private IGenericRepository<ChatMessage>? _chatMessages;
        private IGenericRepository<StoreLocation>? _storeLocations;

        public UnitOfWork(FlowerShopAppContext context)
        {
            _context = context;
        }

        public IGenericRepository<User> Users
        {
            get
            {
                return _users ??= new GenericRepository<User>(_context);
            }
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                return _products ??= new GenericRepository<Product>(_context);
            }
        }

        public IGenericRepository<ProductImage> ProductImages
        {
            get
            {
                return _productImages ??= new GenericRepository<ProductImage>(_context);
            }
        }

        public IGenericRepository<Category> Categories
        {
            get
            {
                return _categories ??= new GenericRepository<Category>(_context);
            }

        }

        public IGenericRepository<Cart> Carts
        {
            get
            {
                return _carts ??= new GenericRepository<Cart>(_context);
            }
        }

        public IGenericRepository<CartItem> CartItems
        {
            get
            {
                return _cartItems ??= new GenericRepository<CartItem>(_context);
            }
        }

        public IGenericRepository<Order> Orders
        {
            get
            {
                return _orders ??= new GenericRepository<Order>(_context);
            }
        }

        public IGenericRepository<OrderItem> OrderItems
        {
            get
            {
                return _orderItems ??= new GenericRepository<OrderItem>(_context);
            }
        }

        public IGenericRepository<Payment> Payments
        {
            get
            {
                return _payments ??= new GenericRepository<Payment>(_context);
            }

        }

        public IGenericRepository<Notification> Notifications
        {
            get
            {
                return _notifications ??= new GenericRepository<Notification>(_context);
            }
        }

        public IGenericRepository<ChatRoom> ChatRooms
        {
            get
            {
                return _chatRooms ??= new GenericRepository<ChatRoom>(_context);
            }
        }

        public IGenericRepository<ChatMessage> ChatMessages
        {
            get
            {
                return _chatMessages ??= new GenericRepository<ChatMessage>(_context);
            }
        }

        public IGenericRepository<StoreLocation> StoreLocations
        {
            get
            {
                return _storeLocations ??= new GenericRepository<StoreLocation>(_context);
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGlobalTransaction BeginTransaction()
        {
            var dbTransaction = _context.Database.BeginTransaction();
            return new EfGlobalTransaction(dbTransaction);
        }
    }
}
