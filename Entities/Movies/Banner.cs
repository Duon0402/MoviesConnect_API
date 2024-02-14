namespace API.Entities.Movies
{
    public class Banner : Photo
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
