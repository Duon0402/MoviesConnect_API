﻿namespace API.Helpers.Params.Movies
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
        public List<int>? ActorId { get; set; }
        public List<int>? DirectorId { get; set; }
        public string? Purpose { get; set; }
        public double? MinRating { get; set; }
        public double? MaxRating { get; set; }
    }   
}
