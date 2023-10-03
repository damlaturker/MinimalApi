using AutoMapper;
using MinimalApi.Demo.Models;
using MinimalApi.Demo.Models.DTO;

namespace MinimalApi.Demo
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<LocalUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
