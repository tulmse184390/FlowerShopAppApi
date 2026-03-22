using FlowerShopApp.Application.DTOs.Chat;

namespace FlowerShopApp.Application.IServices
{
    public interface IChatService
    {
        Task<List<ChatRoomDto>> GetAllChatRoomsAsync();
        Task<int> GetRoomIdAsync(int userId);
        Task SaveMessageAsync(int roomId, string senderRole, string message);

        Task<List<ChatMessageDto>> GetHistoryAsync(int roomId);
        
        Task<bool> IsRoomAIAssistedAsync(int roomId);
        Task ToggleAIAssistanceAsync(int roomId, bool isAIAssisted);
        
        Task<string> GetUserFullNameAsync(int userId);
    }
}
