namespace API.Entities.Movies.Persons
{
    public class ActorImage : Photo
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
