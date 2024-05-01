namespace API.DTOs.Movies.Directors
{
    public class DirectorInputDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
