using Microsoft.AspNetCore.Mvc;
using SignalR.Helpers;
using SignalR.Models;
using SignalR.Services;


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

        [Authorize]
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
