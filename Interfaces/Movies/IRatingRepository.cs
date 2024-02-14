using API.DTOs.Movies.Ratings;
using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface IRatingRepository
    {
        void AddRating(Rating rating);
        void EditRating(Rating rating);
        void RemoveRating(Rating rating);
        Task<bool> Save();
        Task<Rating> GetRatingForEdit(int ratingId);
        Task<IEnumerable<RatingOutputDto>> GetListRatings(int movieId);
    }
}
