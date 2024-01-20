using API.DTOs.Accounts;
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
        }
    }
}
