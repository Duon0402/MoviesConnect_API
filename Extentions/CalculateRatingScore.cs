using API.DTOs.Movies.Ratings;
using API.Entities.Movies;

namespace API.Extentions
{
    public static class CalculateRatingScoreExtentions
    {
        public static double CalculateRatingScore(this IEnumerable<RatingOutputDto> ratings)
        {
            if (ratings == null || !ratings.Any())
                return 0;

            double totalRating = 0;
            foreach (var rating in ratings)
            {
                totalRating += rating.Score;
            }
            double averageRating = totalRating / ratings.Count();
            averageRating = Math.Round(averageRating, 1);
            return averageRating;
        }
    }
}
