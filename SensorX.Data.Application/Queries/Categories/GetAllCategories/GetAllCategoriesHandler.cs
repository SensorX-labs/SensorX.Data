using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;

namespace SensorX.Data.Application.Queries.Categories.GetAllCategories;

public sealed class GetAllCategoriesHandler(
    IQueryBuilder<Category> _categoryQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetAllCategoriesQuery, Result<List<GetAllCategoriesResponse>>>
{
    public async Task<Result<List<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _categoryQueryBuilder.QueryAsNoTracking;

        var dtoQuery = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllCategoriesResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.ParentId == null ? null : x.ParentId.Value,
                x.Parent != null ? x.Parent.Name : null,
                x.CreatedAt
            ));

        var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

        return Result<List<GetAllCategoriesResponse>>.Success(items);
    }
}
