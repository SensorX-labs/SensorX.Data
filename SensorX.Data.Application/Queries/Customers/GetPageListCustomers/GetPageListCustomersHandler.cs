using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.Pagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersHandler(
    IQueryBuilder<Customer> _customerBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListCustomersQuery, Result<CustomerCursorPagedResult>>
{
    public async Task<Result<CustomerCursorPagedResult>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _customerBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);
            var pagedQuery = sourceQuery.ApplyCursorPagination(
                request,
                x => x.CreatedAt,
                x => x.Id
            )
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id);

            var dtoQuery = pagedQuery.Select(x => new GetPageListCustomersResponse(
                x.Id.Value,
                x.Name,
                x.Code.Value,
                x.TaxCode,
                x.Email.Value,
                x.Phone.Value,
                x.Address,
                x.CreatedAt
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery
                .Take(request.PageSize + 1), cancellationToken);

            var hasNext = items.Count > request.PageSize;
            if (hasNext) items.RemoveAt(request.PageSize);

            var result = new CustomerCursorPagedResult
            {
                Items = items,
                HasNext = hasNext,
                HasPrevious = request.IsPrevious,
                FirstCreatedAt = items.FirstOrDefault()?.CreatedAt,
                FirstId = items.FirstOrDefault()?.Id,
                LastCreatedAt = items.LastOrDefault()?.CreatedAt,
                LastId = items.LastOrDefault()?.Id
            };

            return Result<CustomerCursorPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CustomerCursorPagedResult>.Failure(
                $"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
        }
    }
}