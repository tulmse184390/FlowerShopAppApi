namespace FlowerShopApp.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public string? PaymentMethod { get; set; }

        public string? ShippingAddress { get; set; }

        public string? OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
