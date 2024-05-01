using API.Entities.Users;

namespace API.Entities.Movies.Persons
{
    public class Person : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
