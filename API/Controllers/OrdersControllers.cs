﻿using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(
        IOrderService orderService,
        IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderDto)
    {
        var email = HttpContext.User.RetrieveEmailFromPrincipal();

        var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

        var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

        if (order is null)
        {
            return BadRequest(new ApiResponse(400, "Problem creating order"));
        }

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var email =HttpContext.User.RetrieveEmailFromPrincipal();

        var orders = await _orderService.GetOrdersForUserAsync(email);

        return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderToReturnDto>> GetOrdersForUser(int id)
    {
        var email = HttpContext.User.RetrieveEmailFromPrincipal();

        var order = await _orderService.GetOrderAsync(id, email);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<OrderToReturnDto>(order));
    }

    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

        return Ok(deliveryMethods);
    }
}
