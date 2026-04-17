using SensorX.Data.Domain.Common.Exceptions;
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
        if (string.IsNullOrWhiteSpace(taxCode))
            throw new DomainException("Mã số thuế không được để trống.");
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Địa chỉ không được để trống.");
        TaxCode = taxCode;
        Address = address;
        ShippingInfo = shippingInfo;
    }

    private Customer() : base()
    {
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
        if (string.IsNullOrWhiteSpace(taxCode))
            throw new DomainException("Mã số thuế không được để trống.");
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Địa chỉ không được để trống.");
        base.UpdateProfile(name, phone, email);
        TaxCode = taxCode;
        Address = address;
    }

    public void UpdateShippingInfo(ShippingInfo shippingInfo) => ShippingInfo = shippingInfo;
}