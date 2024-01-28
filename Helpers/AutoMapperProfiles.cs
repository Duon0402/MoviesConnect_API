using API.DTOs.Accounts;
using API.DTOs.Movies.Genre;
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
            // accounts
            CreateMap<RegisterDto, AppUser>().ReverseMap();

            // users
            CreateMap<MemberDto, AppUser>().ReverseMap();
            CreateMap<MemberUpdateDto, AppUser>().ReverseMap();

            // movies
            CreateMap<GenreInputDto, Genre>().ReverseMap();
        }
    }
}
