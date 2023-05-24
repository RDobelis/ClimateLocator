using AutoMapper;

namespace ClimateLocator.Core.Models
{
    public class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<IpLocation, WeatherData>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}
