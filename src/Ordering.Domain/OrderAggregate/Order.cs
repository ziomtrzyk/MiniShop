using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    private readonly List<OrderItem> _orderItems = [];

    public string BuyerId { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Address Address {get;private set;} = null!;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order(){}

    public Order(string buyerId, Address address)
    {
        if (string.IsNullOrWhiteSpace(buyerId))
        {
            throw new OrderingDomainException("Buyer id is required.");
        }

        BuyerId = buyerId;
        Address = address;
        OrderStatus = OrderStatus.Submitted;
        OrderDate = DateTime.UtcNow;
    }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, int units)
    {
        var existingItem = _orderItems.SingleOrDefault(i => i.ProductId == productId);

        if(existingItem is not null)
        {
            existingItem.AddUnits(units);
            return;
        }

        _orderItems.Add(new OrderItem(productId, productName, unitPrice, units));
    }

    public void SetCancelledStatus()
    {
        if(OrderStatus is OrderStatus.Paid or OrderStatus.Shipped)
        {
            throw new OrderingDomainException($"Cannot change the order status from {OrderStatus} to {OrderStatus.Cancelled}");
        }

        OrderStatus = OrderStatus.Cancelled;
    }
    
    public void SetPaidStatus()
    {
        if(OrderStatus is not OrderStatus.StockConfirmed)
        {
            throw new OrderingDomainException($"Cannot change the order status from {OrderStatus} to {OrderStatus.Paid}");
        }

        OrderStatus = OrderStatus.Paid;
    }

    public void SetShippedStatus()
    {
        if(OrderStatus is not OrderStatus.Paid)
        {
            throw new OrderingDomainException($"Cannot change the order status from {OrderStatus} to {OrderStatus.Shipped}");
        }

        OrderStatus = OrderStatus.Shipped;
    }

    public decimal GetTotal() => _orderItems.Sum(i => i.Units * i.UnitPrice);
}