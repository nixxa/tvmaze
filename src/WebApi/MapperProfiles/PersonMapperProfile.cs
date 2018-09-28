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
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Person.Birthday ?? new DateTime(1900,1,1)));
            CreateMap<Person, PersonModel>();
        }
    }
}