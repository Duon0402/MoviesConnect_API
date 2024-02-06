using API.Helpers.Pagination;

namespace API.DTOs.Movies
{
    public class MovieInputDto : IPagedInput
    {
        public string Keyword { get; set; }
        public string SearchBy { get; set; }
        public string OrderBy { get; set; }
    }
}