using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.Entities;

using SignalR.Models;
using SignalR.Services;
using System.Security.Claims;

namespace SignalR.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
            
        }

        
        [HttpGet]
        public IActionResult getAll()
        { 
            return Ok("Hello World !!!");
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response);
        }
    }
}
