namespace API.Entities.Movies.Persons
{
    public class DirectorImage : Photo
    {
        public int DirectorId { get; set; }
        public Director Director { get; set; }
    }
}
