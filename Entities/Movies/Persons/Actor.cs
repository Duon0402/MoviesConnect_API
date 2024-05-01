using API.Entities.Users;

namespace API.Entities.Movies.Persons
{
    public class Actor : Person
    {
        public ICollection<MovieActor> MovieActors { get; set; }
        public ActorImage ActorImage { get; set; }
    }
}
