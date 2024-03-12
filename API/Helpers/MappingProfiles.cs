using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductsToReturnDto>()
            .ForMember(d => d.ProductBrand, p => p.MapFrom(x => x.ProductBrand.Name))
            .ForMember(d => d.ProductType, p => p.MapFrom(x => x.ProductType.Name))
            .ForMember(d => d.PictureUrl, p => p.MapFrom<ProductUrlResolver>());

        CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<AddressDto, Address>();

        CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, p => p.MapFrom(x => x.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, p => p.MapFrom(x => x.DeliveryMethod.Price));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, p => p.MapFrom(x => x.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, p => p.MapFrom(x => x.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, p => p.MapFrom<OrderItemUrlResolver>());
    }
}
