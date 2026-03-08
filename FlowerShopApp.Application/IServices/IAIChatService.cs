using System.Threading.Tasks;

namespace FlowerShopApp.Application.IServices
{
    public interface IAIChatService
    {
        Task<ChatCompletionResult> GetChatResponseAsync(string message, string language = "vi");
    }

    public class ChatCompletionResult
    {
        public bool IsSuccess { get; set; }
        public string? Response { get; set; }
        public string? ErrorMessage { get; set; }
        public ChatDebugInfo DebugInfo { get; set; } = new();
    }

    public class ChatDebugInfo
    {
        public string? Provider { get; set; }
        public string? Model { get; set; }
        public long ResponseTimeMs { get; set; }
        public int HttpStatusCode { get; set; }
    }
}
