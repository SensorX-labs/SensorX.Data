namespace SensorX.Data.Application.Queries.Suppliers.GetAllSuppliers;

public sealed record GetAllSuppliersResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
