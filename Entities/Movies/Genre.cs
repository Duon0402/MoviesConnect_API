namespace API.Entities.Movies
{
    public class Genre : Entity
    {
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}