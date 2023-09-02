using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDTO
    {

        [Required] public string Username { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }

        [Required] public DateOnly? DateOfBirth { get; set; } // Note this must be optional or the required validator will not work
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        [Required] public string Password { get; set; }
    }
}