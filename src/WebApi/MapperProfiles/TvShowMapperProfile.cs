using AutoMapper;
using Kernel.Dto;
using Models;
using WebModels;

namespace WebApi.MapperProfiles
{
    public class TvShowMapperProfile : Profile
    {
        public TvShowMapperProfile()
        {
            CreateMap<Show, TvShow>();
            CreateMap<TvShow, TvShowModel>();
        }
    }
}