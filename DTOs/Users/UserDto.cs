using API.Entities;

namespace API.DTOs.Users
{
    public class UserDto
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
