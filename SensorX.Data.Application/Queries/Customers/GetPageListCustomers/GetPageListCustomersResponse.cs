using SensorX.Data.Application.Common.Dtos.Responses;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string TaxCode { get; set; } = null!;
    public string Address { get; set; } = null!;
    public ShippingInfoDto ShippingInfo { get; set; } = null!;
}

public class ShippingInfoDto
{
    public string WardId { get; set; } = null!;
    public string WardName { get; set; } = null!;
    public string ProvinceId { get; set; } = null!;
    public string ProvinceName { get; set; } = null!;
    public string RecipientName { get; set; } = null!;
    public string RecipientPhone { get; set; } = null!;
    public string RecipientAddress { get; set; } = null!;
}
