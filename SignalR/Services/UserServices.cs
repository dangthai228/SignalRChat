using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR.Entities;
using SignalR.Helpers;
using SignalR.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SignalR.Services
{

    public interface IUserService
    {
        AuthenResponse Authenticate(AuthenRequest request);
        User GetById(int id);
    }
    public class UserServices : IUserService
    {
        private List<User> _users = new List<User> 
        {
            new User { Id = 1,Username="thai",Password="12345"},
            new User { Id = 2,Username="tuan",Password="12345"}
        };
        private readonly AppSettings _appSettings;

        public UserServices(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public AuthenResponse Authenticate(AuthenRequest request)
        {
            
            var user = _users.SingleOrDefault(x => x.Username == request.Username && x.Password == request.Password);
            if(user == null) { return null; }

            var token = generateJwtToken(user);
            return new AuthenResponse(user, token);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
    }
}
