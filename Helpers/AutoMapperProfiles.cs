using API.DTOs.Accounts;
using API.DTOs.Movies;
using API.DTOs.Movies.Certifications;
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

            // movies
            CreateMap<MovieDto, Movie>().ReverseMap();
            CreateMap<CertificationDto, Certification>().ReverseMap();
        }
    }
}
