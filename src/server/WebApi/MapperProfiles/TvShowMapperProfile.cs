using AutoMapper;
using Models;
using WebModels;

namespace WebApi.MapperProfiles
{
    public class TvShowMapperProfile : Profile
    {
        public TvShowMapperProfile()
        {
            CreateMap<TvShow, TvShowModel>();
        }
    }
}