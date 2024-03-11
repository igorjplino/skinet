using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services;
public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IGenericRepository<DeliveryMethod> _dmRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IBasketRepository _basketRepository;

    public OrderService(
        IGenericRepository<Order> orderRepository,
        IGenericRepository<DeliveryMethod> dmRepository,
        IGenericRepository<Product> productRepository,
        IBasketRepository basketRepository)
    {
        _orderRepository = orderRepository;
        _dmRepository = dmRepository;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var productItem = await _productRepository.GetByIdAsync(item.Id);

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);

            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

            items.Add(orderItem);
        }

        var deliveryMethod = await _dmRepository.GetByIdAsync(deliveryMethodId);

        var subtotal = items.Sum(x => x.Price * x.Quantity);

        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

        return order;
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderAsync(int id, string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }
}
