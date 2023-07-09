using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MessageApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int SenderId, int ReceiverId, string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", SenderId, ReceiverId, message);
        }
    }
}

