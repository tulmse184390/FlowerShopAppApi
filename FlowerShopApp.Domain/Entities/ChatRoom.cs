namespace FlowerShopApp.Domain.Entities
{
    public class ChatRoom
    {
        public int RoomId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;

        public bool IsAIAssisted { get; set; } = true;

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
