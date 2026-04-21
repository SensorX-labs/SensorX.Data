using SensorX.Data.Application.Common.Pagination;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public record GetPageListCategoriesResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentId,
    DateTimeOffset CreatedAt
);

public class CategoryCursorPagedResult : CursorPagedResult<GetPageListCategoriesResponse> { }