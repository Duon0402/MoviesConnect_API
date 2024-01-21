using API.DTOs.Accounts;
using API.DTOs.Movies;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // accounts
            CreateMap<RegisterDto, AppUser>().ReverseMap();

            // movies
            CreateMap<MovieDto, Movie>().ReverseMap();
        }
    }
}
