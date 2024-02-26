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
        Task<bool> RatingExits(int movieId, int userId);
        Task<Rating> GetRatingForEdit(int movieId, int userId);
        Task<RatingOutputDto> GetRating(int movieId, int userId);
        Task<IEnumerable<RatingOutputDto>> GetListRatings(int movieId);
    }
}
