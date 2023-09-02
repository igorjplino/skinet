using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class ProductUrlResolver : IValueResolver<Product, ProductsToReturnDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(Product source, ProductsToReturnDto destination, string destMember, ResolutionContext context)
    {
        return _configuration["ApiUrl"] + source.PictureUrl;
    }
}
