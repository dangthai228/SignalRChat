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
        private static List<string> users = new List<string>();
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
            //Console.WriteLine("Send to " + receiveUser);
            return Clients.User(receiveUser).SendAsync("SpecifyMessage", message);
        }
        public override  Task OnConnectedAsync()
        {
            users.Add(Context.UserIdentifier);
            Clients.Others.SendAsync("goOnline", Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            users.Remove(Context.UserIdentifier);
            Clients.Others.SendAsync("goOffline", Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }
        public  Task GetFriendStatus(string user)
        {
            string status = "Offline";
            if (users.Contains(user))
            {
                status = "Online";
            }
            return Clients.Caller.SendAsync("StatusMessage",user, status);
        }
    }
}
