namespace API.DTOs.Movies.Certifications
{
    public class CertificationCreateOrEditDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
    }
}
