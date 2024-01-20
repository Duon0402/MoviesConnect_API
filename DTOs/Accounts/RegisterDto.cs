using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }
        [Required] public string FullName { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string Password { get; set; }
    }
}
