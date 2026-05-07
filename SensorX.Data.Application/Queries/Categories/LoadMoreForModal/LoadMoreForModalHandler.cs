using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

namespace SensorX.Data.Application.Queries.Categories.LoadMoreForModal;

public sealed class LoadMoreForModalHandler(
    IQueryBuilder<Category> _categoryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<LoadMoreForModalQuery, Result<LoadMoreForModalResult>>
{
    public async Task<Result<LoadMoreForModalResult>> Handle(LoadMoreForModalQuery request, CancellationToken cancellationToken)
    {
        var categoryQuery = _categoryBuilder.QueryAsNoTracking;
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            categoryQuery = categoryQuery.Where(x => x.Name.Contains(request.SearchTerm));
        }
        var pagedCategoryBaseQuery = categoryQuery.ApplyLoadMoreWithOrder(request.LastValue.ToCursor<DateTimeOffset>(), x => x.CreatedAt, request.LastId, x => (Guid)x.Id, request.IsDescending);

        var pageSize = request.PageSize ?? 10;
        var items = await _queryExecutor.ToListAsync(pagedCategoryBaseQuery.Take(pageSize + 1), cancellationToken);

        bool hasNext = items.Count > pageSize;
        if (hasNext) items.RemoveAt(items.Count - 1);

        var responseItems = items.Select(x => new LoadMoreForModalResponse(
            (Guid)x.Id,
            x.Name,
            x.Description,
            x.CreatedAt
        )).ToList();

        var lastItem = responseItems.LastOrDefault();

        var result = new LoadMoreForModalResult
        {
            Items = responseItems,
            LastId = lastItem?.Id,
            LastValue = lastItem?.CreatedAt.ToString("O"),
            HasNext = hasNext
        };

        return Result<LoadMoreForModalResult>.Success(result);
    }
}