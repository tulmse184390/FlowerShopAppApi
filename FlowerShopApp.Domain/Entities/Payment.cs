namespace FlowerShopApp.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentGateway { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
