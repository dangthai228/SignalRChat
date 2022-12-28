using Microsoft.AspNetCore.Identity;
using SignalR.Entities;

namespace SignalR.Models
{
    public class AuthenResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        
        public AuthenResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}
