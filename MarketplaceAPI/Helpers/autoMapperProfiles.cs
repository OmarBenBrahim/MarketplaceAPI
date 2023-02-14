using AutoMapper;
using MarketplaceAPI.DTOs;
using MarketplaceAPI.Entity;

namespace MarketplaceAPI.Helpers
{
    public class autoMapperProfiles : Profile
    {
        public autoMapperProfiles() 
        {
            CreateMap<Photo, PhotoDto>();
            CreateMap<Product, ProductDto>();
        }
    }
}
