using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignalR.Entities;

using SignalR.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SignalR.Services
{

    public interface IUserService
    {
        AuthenResponse Authenticate(AuthenRequest request);
        User GetById(int id);
    }
    public class UserServices : IUserService
    {
        private string dbstringconnect = "Data Source=THAIND\\MSSQLSERVER01;Initial Catalog=ChatSignalR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private readonly AppSettings _appSettings;

        public UserServices(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public UserServices()
        {
        }

        public AuthenResponse Authenticate(AuthenRequest request)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = dbstringconnect;
                conn.Open();
                try
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Account", conn);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User u = new User((int)reader[0], reader[1].ToString(), reader[2].ToString());
                            if (request.Username == u.Username && request.Password == u.Password)
                            {
                                var token = generateJwtToken(u);
                                return new AuthenResponse(u, token);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("name", user.Username) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public User GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = dbstringconnect;
                conn.Open();
                // use the connection here
                try
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE id = @0", conn);
                    command.Parameters.Add(new SqlParameter("0", id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User((int)reader[0], reader[1].ToString(), reader[2].ToString());
                            return user;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return null;
            }
        }

        public List<User> getListFriend(String name)
        {
            List<User> list = new List<User>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = dbstringconnect;
                conn.Open();
                // use the connection here
                try
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Friend WHERE accountId in (select Id from Account where Username = @0  )", conn);
                    command.Parameters.Add(new SqlParameter("0", name));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        string str = reader[2].ToString();
                        string[] res = str.Split(',');
                        foreach(string v in res)
                        {
                            User u = GetById(Int32.Parse(v));
                            list.Add(u);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;
        }
    }
}
