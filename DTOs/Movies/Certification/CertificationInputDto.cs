using API.Helpers.Pagination;

namespace API.DTOs.Movies.Certification
{
    public class CertificationInputDto : IPagedInput
    {
        public string Keyword { get; set; }
    }
}