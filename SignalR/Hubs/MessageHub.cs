using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalR.Entities;
using SignalR.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;


namespace SignalR.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private UserServices service = new UserServices();
        public  async Task getFriend(string user)
        {
            List<User> listU = service.getListFriend(user);
            string jsonListUser  = JsonConvert.SerializeObject(listU);
            await Clients.Caller.SendAsync("FriendList", jsonListUser);
        }

        public async Task Send(string user, string message)
        {
            await Clients.All.SendAsync("Message", user, message);  
        }
        public Task SendPrivateMessage( string receiveUser, string message)
        {
            Console.WriteLine("Send to " + receiveUser);
            return Clients.User(receiveUser).SendAsync("SpecifyMessage", message);
        }
        public override  Task OnConnectedAsync()
        {
            Clients.Others.SendAsync("goOnline", Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Others.SendAsync("goOffline", Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
