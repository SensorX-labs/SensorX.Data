using SensorX.Data.Application.Common.Dtos.Responses;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}