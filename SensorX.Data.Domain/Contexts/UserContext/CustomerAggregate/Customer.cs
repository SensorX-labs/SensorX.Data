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
        string address
    ) : base(id, accountId, code, name, phone, email)
    {
        TaxCode = taxCode;
        Address = address;
    }

    public string TaxCode { get; private set; }
    public string Address { get; private set; }
    public ShippingInfo ShippingInfo { get; private set; }

    public void UpdateProfile(
        string name,
        Phone phone,
        Email email,
        string taxCode,
        string address
    )
    {
        base.UpdateProfile(name, phone, email);
        TaxCode = taxCode;
        Address = address;
    }

    public void UpdateShippingInfo(ShippingInfo shippingInfo) => ShippingInfo = shippingInfo;
}