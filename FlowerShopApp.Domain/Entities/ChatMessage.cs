namespace FlowerShopApp.Domain.Entities
{
    public class ChatMessage
    {
        public int MessageId { get; set; }

        public int RoomId { get; set; }

        public string? SenderRole { get; set; }

        public string? Message { get; set; }

        public DateTime SentAt { get; set; }

        public virtual ChatRoom ChatRoom { get; set; } = null!;
    }
}
