using API.Helpers.Pagination;

namespace API.DTOs.Movies.Genres
{
    public class GenreInputDto : PagedInput
    {
        public string Keyword { get; set; }
    }
}
