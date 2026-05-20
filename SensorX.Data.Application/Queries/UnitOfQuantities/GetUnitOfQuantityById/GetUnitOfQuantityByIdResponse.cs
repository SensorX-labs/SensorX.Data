namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetUnitOfQuantityById;

public sealed record GetUnitOfQuantityByIdResponse(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
