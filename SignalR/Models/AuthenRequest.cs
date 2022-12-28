using System.ComponentModel.DataAnnotations;

namespace SignalR.Models
{
    public class AuthenRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
