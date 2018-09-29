using System;
using AutoMapper;
using Kernel.Dto;
using Models;
using WebModels;

namespace WebApi.MapperProfiles
{
    public class PersonMapperProfile : Profile
    {
        public PersonMapperProfile()
        {
            CreateMap<Cast, Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Person.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Person.Name))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => GetBirthday(src.Person.Birthday)));
            CreateMap<Person, PersonModel>();
        }

        private static DateTime GetBirthday(DateTime? source)
        {
            if (!source.HasValue) return new DateTime(1900,1,1,0,0,0, DateTimeKind.Utc);
            var result = new DateTime(source.Value.Date.Ticks, DateTimeKind.Utc);
            return result;
        }
    }
}