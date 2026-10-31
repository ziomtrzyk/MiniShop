using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.OrderAggregate;

public class OrderItem : Entity
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Units { get; private set; }

    public OrderItem(int productId, string productName, decimal unitPrice, int units)
    {
        if(units < 1)
        {
            throw new OrderingDomainException("Units cannot be less than one.");
        }
        if(unitPrice < 0)
        {
            throw new OrderingDomainException("UnitPrice cannot be less than 0.");
        }
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new OrderingDomainException("ProductName is required.");
        }

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Units = units;
    }

    public void AddUnits(int units)
    {
        if(units < 1)
        {
            throw new OrderingDomainException("Units cannot be less than one.");
        }
        Units += units;
    }
}