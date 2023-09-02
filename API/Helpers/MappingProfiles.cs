using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductsToReturnDto>()
            .ForMember(d => d.ProductBrand, p => p.MapFrom(x => x.ProductBrand.Name))
            .ForMember(d => d.ProductType, p => p.MapFrom(x => x.ProductType.Name))
            .ForMember(d => d.PictureUrl, p => p.MapFrom<ProductUrlResolver>());
    }
}
