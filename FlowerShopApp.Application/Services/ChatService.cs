using FlowerShopApp.Application.DTOs.Chat;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Entities;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetRoomIdAsync(int userId)
        {
            var room = await _unitOfWork.ChatRooms.Entities
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (room == null)
            {
                room = new ChatRoom { UserId = userId, CreatedAt = DateTime.UtcNow };
                _unitOfWork.ChatRooms.Add(room);
                await _unitOfWork.CompleteAsync();
            }
            return room.RoomId;
        }

        public async Task SaveMessageAsync(int roomId, string senderRole, string message)
        {
            var chatMsg = new ChatMessage
            {
                RoomId = roomId,
                SenderRole = senderRole,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            _unitOfWork.ChatMessages.Add(chatMsg);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<ChatMessageDto>> GetHistoryAsync(int roomId)
        {
            return await _unitOfWork.ChatMessages.Entities
                .Where(m => m.RoomId == roomId)
                .OrderBy(m => m.SentAt)
                .Select(m => new ChatMessageDto
                {
                    SenderRole = m.SenderRole,
                    Message = m.Message,
                    SentAt = m.SentAt
                })
                .ToListAsync();
        }

        public async Task<bool> IsRoomAIAssistedAsync(int roomId)
        {
            var room = await _unitOfWork.ChatRooms.Entities.FirstOrDefaultAsync(r => r.RoomId == roomId);
            return room?.IsAIAssisted ?? true;
        }

        public async Task ToggleAIAssistanceAsync(int roomId, bool isAIAssisted)
        {
            var room = await _unitOfWork.ChatRooms.Entities.FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room != null)
            {
                room.IsAIAssisted = isAIAssisted;
                _unitOfWork.ChatRooms.Update(room);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
