using System.Collections.Concurrent;
using System.Security.Claims;
using LiveChat.Entities;
using LiveChat.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace LiveChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IChatRoomService _chatRoomService;
        private static readonly ConcurrentDictionary<string, (string Username, Guid? UserId)> _connectedUsers
           = new ConcurrentDictionary<string, (string Username, Guid? UserId)>();

        public ChatHub(IUserService userService, IMessageService messageService, IChatRoomService chatRoomService)
        {
            _userService = userService;
            _messageService = messageService;
            _chatRoomService = chatRoomService;
        }


        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var (username, userId) = await GetSenderInfo(user.Identity.Name!);
                _connectedUsers[Context.ConnectionId] = (username, userId);
            }
            else
            {
                Console.WriteLine("Invitado conectado.");
            };
            await base.OnConnectedAsync();
        }

        public async Task RegisterUser(string username)
        {
            _connectedUsers[Context.ConnectionId] = (username, null);
            await Clients.All.SendAsync("UsersOnline", GetConnectedUsersList());
        }
        public async Task SendMessage(string username, string content, string type)
        {
            var (senderUsername, senderId) = await GetSenderInfo(username);

            if (!Enum.TryParse<MessageType>(type, true, out var messageType))
            {
                messageType = MessageType.Text;
            }
            Message message = await _messageService.SaveGroupMessage(senderUsername, content, messageType, senderId);
            await Clients.All.SendAsync("ReceiveMessage", senderUsername, content, type, message.SentAt);
        }

        public async Task SendPrivateMessage(string senderName, string recipientName, string content, string type)
        {
            var (senderUsername, senderId) = await GetSenderInfo(senderName);

            User? recipientUser = await _userService.GetByUsername(recipientName);
            ChatRoom room = await _chatRoomService.GetOrCreatePrivateRoom(senderId, senderUsername, recipientUser?.Id, recipientName);
            if (!Enum.TryParse<MessageType>(type, true, out var messageType))
            {
                messageType = MessageType.Text;
            }
            var message = await _messageService.SavePrivateMessage(room.Id, senderUsername, content, messageType, senderId);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
            await Clients.Group(room.Id.ToString()).SendAsync("ReceivePrivateMessage", senderUsername, content, type, message.SentAt);
        }


        private async Task<(string senderUsername, Guid? senderId)> GetSenderInfo(string fallbackName)
        {
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                string? email = Context.User.FindFirst(ClaimTypes.Email)?.Value;
                if (email != null)
                {
                    User? user = await _userService.GetByEmail(email);
                    return (user?.Username ?? "Invitado", user?.Id);
                }
            }
            return (fallbackName, null);
        }
        private List<object> GetConnectedUsersList()
        {
            return _connectedUsers.Values
                .Select(u => u.UserId.HasValue
                    ? (object)new { username = u.Username, userId = u.UserId }
                    : (object)new { username = u.Username })
                .ToList();
        }
    }
}
