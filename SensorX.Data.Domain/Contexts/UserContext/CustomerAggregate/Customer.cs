using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

public class Customer : User<CustomerId>
{
    public Customer(
        CustomerId id,
        AccountId accountId,
        Code code,
        string name,
        Phone phone,
        Email email,
        string taxCode,
        string address,
        ShippingInfo shippingInfo
    ) : base(id, accountId, code, name, phone, email)
    {
        TaxCode = taxCode;
        Address = address;
        ShippingInfo = shippingInfo;
    }

    public string TaxCode { get; private set; }
    public string Address { get; private set; }
    public ShippingInfo ShippingInfo { get; private set; }

    public void UpdateProfile(
        string name,
        Phone phone,
        Email email,
        string taxCode,
        string address,
        ShippingInfo shippingInfo
    )
    {
        base.UpdateProfile(name, phone, email);
        TaxCode = taxCode;
        Address = address;
        ShippingInfo = shippingInfo;
    }
}