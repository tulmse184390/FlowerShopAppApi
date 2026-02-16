namespace FlowerShopApp.Domain.Entities
{
    public class Cart
    {
        public int CartId { get; set; } 

        public int UserId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
