using API.DTOs.Accounts;
using API.DTOs.Movies.Actor;
using API.DTOs.Movies.Certifications;
using API.DTOs.Movies.Directors;
using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.DTOs.Movies.Ratings;
using API.DTOs.Photos;
using API.DTOs.Points;
using API.DTOs.Reports;
using API.DTOs.Users;
using API.DTOs.Users.Member;
using API.DTOs.Vouchers;
using API.Entities;
using API.Entities.Movies;
using API.Entities.Movies.Persons;
using API.Entities.Users;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //vouchers
            CreateMap<Voucher, VoucherOutputDto>().ReverseMap();

            // persons
            CreateMap<Actor, ActorOutputDto>().ReverseMap();
            CreateMap<Actor, ActorInputDto>().ReverseMap();
            CreateMap<Director, DirectorInputDto>().ReverseMap();
            CreateMap<Director, DirectorOutputDto>().ReverseMap();

            // points
            CreateMap<PointTransaction, PointTransactionInputDto>().ReverseMap();

            // report
            CreateMap<Report, ReportDto>().ReverseMap();

            // rating
            CreateMap<RatingAddOrEditDto, Rating>().ReverseMap();
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
            CreateMap<CertificationCreateOrEditDto, Certification>().ReverseMap();
        }
    }
}