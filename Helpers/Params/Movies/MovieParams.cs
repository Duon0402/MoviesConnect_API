namespace API.Helpers.Params.Movies
{
    public class MovieParams
    {
        public string? Keyword { get; set; }
        public string? OrderBy { get; set; }
        public string SortOrder { get; set; } = "asc";
        public string? Status { get; set; }
        public int? PageSize { get; set; }
        public List<int>? CertificationId { get; set; }
        public List<int>? GenreId { get; set; }
        public string? Purpose { get; set; }
    }
}
