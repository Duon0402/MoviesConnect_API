using API.Helpers.Pagination;

namespace API.DTOs.Movies.Genres
{
    public class GenreInputDto : IPagedInput
    {
        public string? Keyword { get; set; }
    }
}