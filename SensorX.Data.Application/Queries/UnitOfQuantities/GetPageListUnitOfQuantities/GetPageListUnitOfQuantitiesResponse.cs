namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetPageListUnitOfQuantities;

public sealed record GetPageListUnitOfQuantitiesResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
