using FlowerShopApp.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowerShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomId = await _chatService.GetRoomIdAsync(userId);

            var history = await _chatService.GetHistoryAsync(roomId);
            return Ok(history);
        }

        [HttpPost("{roomId}/takeover")]
        public async Task<IActionResult> Takeover(int roomId)
        {
            await _chatService.ToggleAIAssistanceAsync(roomId, false);
            return Ok(new { message = "Staff has taken over the chat." });
        }

        [HttpPost("{roomId}/release")]
        public async Task<IActionResult> Release(int roomId)
        {
            await _chatService.ToggleAIAssistanceAsync(roomId, true);
            return Ok(new { message = "AI has resumed the chat." });
        }
    }
}
