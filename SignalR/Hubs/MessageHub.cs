using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class MessageHub : Hub
    {
        public async Task Send(string user, string message)
        {
            await Clients.All.SendAsync("Message", user, message);  
        }
        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("Message", message);
        }
    }
}
