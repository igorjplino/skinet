﻿using API.Dtos;
using AutoMapper;
using Core.Entities.OrderAggregate;

namespace API.Helpers;

public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        return _configuration["ApiUrl"] + source.ItemOrdered.PictureUrl;
    }
}
