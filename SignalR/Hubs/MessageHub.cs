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
    }
}
