using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using FlowerShopApp.Api.Hubs;

namespace FlowerShopApp.Api.Controllers         
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "ADMIN, STAFF")] 
        [HttpGet("chat_room")]
        public async Task<IActionResult> GetChatRoom()
        {
            var allRooms = await _chatService.GetAllChatRoomsAsync();
            return Ok(allRooms);
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomId = await _chatService.GetRoomIdAsync(userId);

            var history = await _chatService.GetHistoryAsync(roomId);
            return Ok(history);
        }

        [HttpGet("{roomId}/history")]
        public async Task<IActionResult> GetHistoryByRoomId(int roomId)
        {
            var history = await _chatService.GetHistoryAsync(roomId);
            return Ok(history);
        }

        [HttpPost("{roomId}/takeover")]
        public async Task<IActionResult> Takeover(int roomId)
        {
            await _chatService.ToggleAIAssistanceAsync(roomId, false);
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveToggleAI", false);
            return Ok(new { message = "Staff has taken over the chat." });
        }

        [HttpPost("{roomId}/release")]
        public async Task<IActionResult> Release(int roomId)
        {
            await _chatService.ToggleAIAssistanceAsync(roomId, true);
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("ReceiveToggleAI", true);
            return Ok(new { message = "AI has resumed the chat." });
        }
    }
}
