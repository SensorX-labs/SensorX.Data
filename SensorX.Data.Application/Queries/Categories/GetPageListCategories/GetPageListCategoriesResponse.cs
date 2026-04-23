using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public sealed record GetPageListCategoriesResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentId,
    DateTimeOffset CreatedAt
);

public sealed class CategoryOffsetPagedResult : OffsetPagedResult<GetPageListCategoriesResponse> { }