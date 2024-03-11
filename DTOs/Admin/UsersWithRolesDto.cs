namespace API.DTOs.Admin
{
    public class UsersWithRolesDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
