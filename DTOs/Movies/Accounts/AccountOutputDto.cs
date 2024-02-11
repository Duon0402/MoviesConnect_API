namespace API.DTOs.Movies.Accounts
{
    public class AccountOutputDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
        public int? AvatarId { get; set; }
        public string Token { get; set; }
    }
}