namespace SensorX.Data.Application.Queries.Suppliers.GetPageListSuppliers;

public sealed record GetPageListSuppliersResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
