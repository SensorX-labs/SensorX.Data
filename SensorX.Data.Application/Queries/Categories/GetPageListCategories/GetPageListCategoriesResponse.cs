using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public record GetPageListCategoriesResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentId,
    DateTimeOffset CreatedAt
);

public class CategoryOffsetPagedResult : OffsetPagedResult<GetPageListCategoriesResponse> { }