namespace FlowerShopApp.Application.DTOs.Chat
{
    public class ChatMessageDto
    {
        public string SenderRole { get; set; } 
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
