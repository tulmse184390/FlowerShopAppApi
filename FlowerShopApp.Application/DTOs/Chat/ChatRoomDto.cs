using FlowerShopApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopApp.Application.DTOs.Chat
{
    public class ChatRoomDto
    {
        public int RoomId { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public bool IsAIAssisted { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
