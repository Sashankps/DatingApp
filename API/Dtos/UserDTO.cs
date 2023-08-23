using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }
        public string Token { get; set; }

    }
}