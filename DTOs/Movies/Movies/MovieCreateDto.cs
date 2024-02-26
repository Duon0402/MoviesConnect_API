﻿using API.Entities.Movies;

namespace API.DTOs.Movies.Movie
{
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Status { get; set; }
        public int CertificationId { get; set; }
        public List<int> GenreIds { get; set; }
    }
}