using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FlowerShopApp.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
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

        public async Task SendMessageToShop(string message)
        {
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomId = await _chatService.GetRoomIdAsync(userId);

            await _chatService.SaveMessageAsync(roomId, "User", message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "User", message);
        }

        public async Task SendMessageToUser(int roomId, string message)
        {
            await _chatService.SaveMessageAsync(roomId, "Admin", message);
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", "Admin", message);
        }
    }
}
