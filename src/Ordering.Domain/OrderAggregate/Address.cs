using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.OrderAggregate;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    public Address(string street, string city, string state, string country, string zipCode)
    {
        if(string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country) || string.IsNullOrWhiteSpace(zipCode))
        {
            
            throw new OrderingDomainException("Street, city, country and zip code are required.");
        }
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }
}
