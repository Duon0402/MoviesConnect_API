using API.Entities.Movies.Persons;

namespace API.DTOs.Movies.Actor
{
    public class ActorOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ActorImage ActorImage { get; set; }
    }
}
