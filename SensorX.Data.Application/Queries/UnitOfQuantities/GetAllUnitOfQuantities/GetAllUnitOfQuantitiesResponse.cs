namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetAllUnitOfQuantities;

public sealed record GetAllUnitOfQuantitiesResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
