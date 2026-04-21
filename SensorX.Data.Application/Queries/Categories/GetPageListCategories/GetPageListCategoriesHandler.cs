using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public class GetPageListCategoriesHandler(
    IQueryBuilder<Category> categoryQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetPageListCategoriesQuery, Result<CategoryCursorPagedResult>>
{
    public async Task<Result<CategoryCursorPagedResult>> Handle(GetPageListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = categoryQueryBuilder.QueryAsNoTracking;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(x => x.Name.Contains(request.SearchTerm));
            query = query.Where(x => x.Description.Contains(request.SearchTerm));
        }
        query = query.ApplyCursorPagination(request, x => x.CreatedAt, x => x.Id);
        var dtoQuery = query.Select(x => new GetPageListCategoriesResponse(
            x.Id.Value,
            x.Name,
            x.Description,
            x.ParentId == null ? null : x.ParentId.Value,
            x.CreatedAt
        ));

        var items = await queryExecutor.ToListAsync(dtoQuery.Take(request.PageSize + 1), cancellationToken);

        var hasNext = items.Count > request.PageSize;
        if (hasNext) items.RemoveAt(request.PageSize);

        var result = new CategoryCursorPagedResult
        {
            Items = items,
            HasNext = hasNext,
            HasPrevious = request.IsPrevious,
            FirstCreatedAt = items.FirstOrDefault()?.CreatedAt,
            FirstId = items.FirstOrDefault()?.Id,
            LastCreatedAt = items.LastOrDefault()?.CreatedAt,
            LastId = items.LastOrDefault()?.Id
        };

        return Result<CategoryCursorPagedResult>.Success(result);
    }
}