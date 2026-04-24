using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.QueryExtensions.Search;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public sealed class GetPageListCustomersHandler(
    IQueryBuilder<Customer> _customerBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListCustomersQuery, Result<CustomerOffsetPagedResult>>
{
    public async Task<Result<CustomerOffsetPagedResult>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _customerBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

            var totalCount = await _queryExecutor.CountAsync(sourceQuery, cancellationToken);

            var pagedQuery = sourceQuery
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .ApplyOffsetPagination(request);

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

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new CustomerOffsetPagedResult
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<CustomerOffsetPagedResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CustomerOffsetPagedResult>.Failure(
                $"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
        }
    }
}