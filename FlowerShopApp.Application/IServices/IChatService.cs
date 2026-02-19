using FlowerShopApp.Application.DTOs.Chat;

namespace FlowerShopApp.Application.IServices
{
    public interface IChatService
    {
        Task<int> GetRoomIdAsync(int userId);
        Task SaveMessageAsync(int roomId, string senderRole, string message);

        Task<List<ChatMessageDto>> GetHistoryAsync(int roomId);
    }
}
