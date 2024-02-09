namespace API.DTOs.Movies.Certifications
{
    public class CertificationOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
    }
}