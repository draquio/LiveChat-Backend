using LiveChat.Context;
using LiveChat.Entities;
using LiveChat.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiveChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public ChatHub(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public async Task SendMessage(string username, string content, string type)
        {
            User user = await _userService.GetOrCreateGuestUser(username);
            Message message = await _messageService.SaveGroupMessage(user, content);
            await Clients.All.SendAsync("ReceiveMessage", user.Username, content, type, message.SentAt);
        }
    }
}
