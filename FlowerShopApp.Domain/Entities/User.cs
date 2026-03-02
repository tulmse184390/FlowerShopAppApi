namespace FlowerShopApp.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
    }
}
