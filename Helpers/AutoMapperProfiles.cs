using API.DTOs.Accounts;
using API.DTOs.Movies.Certifications;
using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.DTOs.Movies.Ratings;
using API.DTOs.Photos;
using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.Entities.Movies;
using API.Entities.Users;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            // rating
            CreateMap<RatingCreateDto, Rating>().ReverseMap();
            CreateMap<RatingUpdateDto, Rating>().ReverseMap();
            CreateMap<RatingOutputDto, Rating>().ReverseMap();

            // photos
            CreateMap<AvatarDto, Avatar>().ReverseMap();
            CreateMap<BannerDto, Banner>().ReverseMap();

            // movies 
            CreateMap<MovieCreateDto, Movie>().ReverseMap();
            CreateMap<MovieUpdateDto, Movie>().ReverseMap();
            CreateMap<MovieOutputDto, Movie>().ReverseMap();
            CreateMap<ListMoviesOutputDto, Movie>().ReverseMap();

            // accounts
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            // users
            CreateMap<MemberDto, AppUser>().ReverseMap();
            CreateMap<MemberUpdateDto, AppUser>().ReverseMap();

            // genres
            CreateMap<GenreCreateDto, Genre>().ReverseMap();
            CreateMap<GenreOutputDto, Genre>().ReverseMap();
            CreateMap<GenreUpdateDto, Genre>().ReverseMap();

            // certifications
            CreateMap<CertificationOutputDto, Certification>().ReverseMap();
        }
    }
}