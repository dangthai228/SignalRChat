using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalR.Entities;
using SignalR.Services;
using System;
using System.Collections.Generic;
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
            return Clients.User(receiveUser).SendAsync("Message", message);
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            await Clients.All.SendAsync("Message","",$"{Context.User.Identity.Name} joined.");
            await base.OnConnectedAsync();
        }


    }
}
