using MediatR;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public record GetPageListCategoriesQuery : CursorPagedQuery, IRequest<Result<CategoryCursorPagedResult>>
{
    public string? SearchTerm { get; init; }
}
