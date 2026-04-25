namespace SensorX.Data.Application.Queries.Categories.GetAllCategories;

public sealed record GetAllCategoriesResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentId,
    string? ParentName,
    DateTimeOffset CreatedAt
);
