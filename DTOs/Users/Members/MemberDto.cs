using API.DTOs.Photos;
using API.Entities;
using API.Entities.Users;

namespace API.DTOs.Users
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsPublic { get; set; }
        public AvatarDto Avatar { get; set; }
    }
}