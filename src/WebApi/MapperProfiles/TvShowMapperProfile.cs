using System.Linq;
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
            CreateMap<TvShow, TvShowModel>()
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.Casts.OrderByDescending(x => x.Birthday)));
        }
    }
}