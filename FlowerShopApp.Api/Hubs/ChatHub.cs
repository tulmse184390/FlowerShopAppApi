using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace FlowerShopApp.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IAIChatService _aiChatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, IAIChatService aiChatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _aiChatService = aiChatService;
            _logger = logger;
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
            _logger.LogInformation("Received message from user ID {UserId} intended for AI: '{Message}'", userId, message);
            
            var roomId = await _chatService.GetRoomIdAsync(userId);
            var fullName = await _chatService.GetUserFullNameAsync(userId);
            if (fullName.Length > 13) fullName = fullName.Substring(0, 13);
            string userTitle = $"{fullName} (User)";

            // Ensure this room is in AI-assisted mode.
            await _chatService.ToggleAIAssistanceAsync(roomId, true);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveToggleAI", true);

            await _chatService.SaveMessageAsync(roomId, userTitle, message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userTitle, message);

            var isAIAssisted = await _chatService.IsRoomAIAssistedAsync(roomId);
            if (isAIAssisted)
            {
                var aiResponse = await _aiChatService.GetChatResponseAsync(message);
                _logger.LogInformation("AI Response Received - Success: {IsSuccess}, Content Length: {Length}, Error: {Error}", aiResponse.IsSuccess, aiResponse.Response?.Length ?? 0, aiResponse.ErrorMessage ?? "None");

                if (aiResponse.IsSuccess && !string.IsNullOrEmpty(aiResponse.Response))
                {
                    string aiModel = aiResponse.DebugInfo?.Model ?? "Assistant";
                    if (aiModel.Length > 15) aiModel = aiModel.Substring(0, 15);
                    string aiTitle = $"{aiModel} (AI)";
                    
                    await _chatService.SaveMessageAsync(roomId, aiTitle, aiResponse.Response);
                    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", aiTitle, aiResponse.Response);
                }
                else
                {
                    string aiModel = aiResponse.DebugInfo?.Model ?? "Assistant";
                    if (aiModel.Length > 15) aiModel = aiModel.Substring(0, 15);
                    string aiTitle = $"{aiModel} (AI)";
                    string errorMsg = "Sorry, I am currently unable to process your request. Please try again later. (" + (aiResponse.ErrorMessage ?? "Unknown Error") + ")";
                    
                    await _chatService.SaveMessageAsync(roomId, aiTitle, errorMsg);
                    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", aiTitle, errorMsg);
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
            var fullName = await _chatService.GetUserFullNameAsync(userId);
            if (fullName.Length > 13) fullName = fullName.Substring(0, 13);
            string userTitle = $"{fullName} (User)";

            // Disable AI for this room while customer is chatting with staff.
            await _chatService.ToggleAIAssistanceAsync(roomId, false);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveToggleAI", false);

            await _chatService.SaveMessageAsync(roomId, userTitle, message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userTitle, message);
            // No AI reply — staff will respond manually via SendMessageToUser
        }

        /// <summary>
        /// Staff sends a reply to a customer room.
        /// </summary>
        public async Task SendMessageToUser(int roomId, string message)
        {
            var staffIdStr = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var staffTitle = "Staff";
            if (!string.IsNullOrEmpty(staffIdStr) && int.TryParse(staffIdStr, out var staffId))
            {
                var staffName = await _chatService.GetUserFullNameAsync(staffId);
                if (staffName.Length > 12) staffName = staffName.Substring(0, 12);
                staffTitle = $"{staffName} (Staff)";
            }
            
            await _chatService.SaveMessageAsync(roomId, staffTitle, message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", staffTitle, message);
        }

        /// <summary>
        /// Staff joins a specific room group to listen to messages
        /// </summary>
        public async Task JoinRoomGroup(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        /// <summary>
        /// Staff leaves a specific room group
        /// </summary>
        public async Task LeaveRoomGroup(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }
}
