using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using WebApi.MapperProfiles;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            Mapper.Initialize(cfg => 
            {
                cfg.AddProfile(new TvShowMapperProfile());
            });
            services.AddSingleton(Mapper.Instance);
        }
    }
}