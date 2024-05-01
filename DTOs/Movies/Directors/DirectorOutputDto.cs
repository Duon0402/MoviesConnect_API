using API.Entities.Movies.Persons;

namespace API.DTOs.Movies.Directors
{
    public class DirectorOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DirectorImage DirectorImage { get; set; }
    }
}
