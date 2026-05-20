namespace SensorX.Data.Application.Queries.Suppliers.GetSupplierById;

public sealed record GetSupplierByIdResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
