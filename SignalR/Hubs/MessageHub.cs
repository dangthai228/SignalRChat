using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    [Helpers.Authorize]
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
