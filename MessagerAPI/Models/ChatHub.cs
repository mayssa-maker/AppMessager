using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MessageApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", senderId, receiverId, message);
        }
    }
}

