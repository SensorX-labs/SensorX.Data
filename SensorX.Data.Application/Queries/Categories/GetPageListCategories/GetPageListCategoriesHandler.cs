using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

namespace SensorX.Data.Application.Queries.Categories.GetPageListCategories;

public sealed class GetPageListCategoriesHandler(
    IQueryBuilder<Category> categoryQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetPageListCategoriesQuery, Result<OffsetPagedResult<GetPageListCategoriesResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListCategoriesResponse>>> Handle(GetPageListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = categoryQueryBuilder.QueryAsNoTracking;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(x => x.Name.Contains(term) || x.Description.Contains(term));
        }

        var totalCount = await queryExecutor.CountAsync(query, cancellationToken);

        query = query.ApplyOffsetPagination(request);

        var dtoQuery = query.Select(x => new GetPageListCategoriesResponse(
            x.Id.Value,
            x.Name,
            x.Description,
            x.ParentId == null ? null : x.ParentId.Value,
            x.Parent != null ? x.Parent.Name : null,
            x.CreatedAt
        ));

        var items = await queryExecutor.ToListAsync(dtoQuery, cancellationToken);

        var result = new OffsetPagedResult<GetPageListCategoriesResponse>
        {
            Items = items,
            PageNumber = request.PageNumber ?? 1,
            PageSize = request.PageSize ?? 10,
            TotalCount = totalCount
        };

        return Result<OffsetPagedResult<GetPageListCategoriesResponse>>.Success(result);
    }
}