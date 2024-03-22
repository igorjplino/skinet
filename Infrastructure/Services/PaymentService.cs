using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;
public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IBasketRepository _basketRepository;
    private readonly IConfiguration _configuration;

    public PaymentService(
        IUnitOfWork uow,
        IBasketRepository basketRepository,
        IConfiguration configuration)
    {
        _uow = uow;
        _basketRepository = basketRepository;
        _configuration = configuration;
    }

    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

        var basket = await _basketRepository.GetBasketAsync(basketId);

        if (basket is null)
        {
            return null;
        }

        var shippingPrice = 0m;

        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

            shippingPrice = deliveryMethod.Price;
        }

        foreach (var item in basket.Items)
        {
            var productItem = await _uow.Repository<Product>().GetByIdAsync(item.Id);

            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

        var service = new PaymentIntentService();

        PaymentIntent intent;

        var amount = (long)basket.Items.Sum(x => x.Quantity * (x.Price * 100)) + ((long)shippingPrice * 100);

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            intent = await service.CreateAsync(options);
            
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = amount
            };

            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);

        return basket;
    }

    public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

        var order = await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);

        if (order is null)
        {
            return null;
        }

        order.Status = OrderStatus.PaymentFaild;

        await _uow.Complete();

        return order;
    }

    public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

        var order = await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);

        if (order is null)
        {
            return null;
        }

        order.Status = OrderStatus.PaymentReceived;

        await _uow.Complete();

        return order;
    }
}
