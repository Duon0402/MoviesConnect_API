using API.Helpers.Pagination;

namespace API.DTOs.Movies.Certifications
{
    public class CertificationInputDto : IPagedInput
    {
        public string? Keyword { get; set; }
    }
}