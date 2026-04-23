using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public sealed record GetPageListCategoriesQuery : OffsetPagedQuery, IRequest<Result<CategoryOffsetPagedResult>>
{
    public string? SearchTerm { get; init; }
}
