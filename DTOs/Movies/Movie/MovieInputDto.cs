using API.Helpers.Pagination;

namespace API.DTOs.Movies.Movie
{
    public class MovieInputDto : IPagedInput
    {
        public string? Keyword { get; set; }
        public string? SearchBy { get; set; }
        public string? OrderBy { get; set; }
    }
}