using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly IBasketRepository _basketRepository;

    public OrderService(
        IUnitOfWork unitOfWork, IBasketRepository basketRepository)
    {
        _uow = unitOfWork;
        _basketRepository = basketRepository;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var productItem = await _uow.Repository<Product>().GetByIdAsync(item.Id);

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

            items.Add(orderItem);
        }

        var deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        var subtotal = items.Sum(x => x.Price * x.Quantity);

        var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);

        var order = await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);

        if (order is not null)
        {
            order.ShipToAddress = shippingAddress;
            order.DeliveryMethod = deliveryMethod;
            order.SubTotal = subtotal;

            _uow.Repository<Order>().Update(order);
        }
        else
        {
            order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);

            _uow.Repository<Order>().Add(order);
        }

        await _uow.Complete();

        _uow.Repository<Order>().Add(order);

        await _uow.Complete();

        await _basketRepository.DeleteBasketAsync(basketId);

        _uow.Repository<Order>().Add(order);

        await _uow.Complete();

        await _basketRepository.DeleteBasketAsync(basketId);

        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _uow.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order> GetOrderAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

        return await _uow.Repository<Order>().GetEntityWithSpecAsync(spec);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

        return await _uow.Repository<Order>().ListAsync(spec);
    }
}
