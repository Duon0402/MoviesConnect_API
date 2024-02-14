namespace API.Entities.Users
{
    public class Avatar : Photo
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
