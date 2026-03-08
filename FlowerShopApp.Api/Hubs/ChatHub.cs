using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FlowerShopApp.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IAIChatService _aiChatService;

        public ChatHub(IChatService chatService, IAIChatService aiChatService)
        {
            _chatService = chatService;
            _aiChatService = aiChatService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var roomId = await _chatService.GetRoomIdAsync(int.Parse(userId));
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Customer sends a message — AI responds automatically.
        /// </summary>
        public async Task SendMessageToShop(string message)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomId = await _chatService.GetRoomIdAsync(userId);

            // Ensure this room is in AI-assisted mode.
            await _chatService.ToggleAIAssistanceAsync(roomId, true);

            await _chatService.SaveMessageAsync(roomId, "User", message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "User", message);

            var isAIAssisted = await _chatService.IsRoomAIAssistedAsync(roomId);
            if (isAIAssisted)
            {
                var aiResponse = await _aiChatService.GetChatResponseAsync(message);
                if (aiResponse.IsSuccess && !string.IsNullOrEmpty(aiResponse.Response))
                {
                    await _chatService.SaveMessageAsync(roomId, "AI", aiResponse.Response);
                    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "AI", aiResponse.Response);
                }
            }
        }

        /// <summary>
        /// Customer sends a message to Staff — no AI reply, waits for human staff to respond.
        /// </summary>
        public async Task SendMessageToStaff(string message)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomId = await _chatService.GetRoomIdAsync(userId);

            // Disable AI for this room while customer is chatting with staff.
            await _chatService.ToggleAIAssistanceAsync(roomId, false);

            await _chatService.SaveMessageAsync(roomId, "User", message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "User", message);
            // No AI reply — staff will respond manually via SendMessageToUser
        }

        /// <summary>
        /// Staff sends a reply to a customer room.
        /// </summary>
        public async Task SendMessageToUser(int roomId, string message)
        {
            await _chatService.SaveMessageAsync(roomId, "Staff", message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "Staff", message);
        }
    }
}
