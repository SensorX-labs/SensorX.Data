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
) : IRequestHandler<GetPageListCustomersQuery, Result<OffsetPagedResult<GetPageListCustomersResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListCustomersResponse>>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var sourceQuery = _customerBuilder.QueryAsNoTracking.ApplySearch(request.SearchTerm);

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var code = request.Code.Trim();
                sourceQuery = sourceQuery.Where(x => ((string)x.Code).Contains(code));
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();
                sourceQuery = sourceQuery.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(request.TaxCode))
            {
                var taxCode = request.TaxCode.Trim();
                sourceQuery = sourceQuery.Where(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var email = request.Email.Trim();
                sourceQuery = sourceQuery.Where(x => ((string)x.Email).Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                var phone = request.Phone.Trim();
                sourceQuery = sourceQuery.Where(x => x.Phone != null && ((string)x.Phone).Contains(phone));
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                var address = request.Address.Trim();
                sourceQuery = sourceQuery.Where(x => x.Address != null && x.Address.Contains(address));
            }

            if (request.CreatedFrom.HasValue)
            {
                var createdFrom = request.CreatedFrom.Value.Date;
                sourceQuery = sourceQuery.Where(x => x.CreatedAt >= createdFrom);
            }

            if (request.CreatedTo.HasValue)
            {
                var createdToExclusive = request.CreatedTo.Value.Date.AddDays(1);
                sourceQuery = sourceQuery.Where(x => x.CreatedAt < createdToExclusive);
            }

            var totalCount = await _queryExecutor.CountAsync(sourceQuery, cancellationToken);

            var pagedQuery = sourceQuery
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .ApplyOffsetPagination(request);

            var dtoQuery = pagedQuery.Select(x => new GetPageListCustomersResponse(
                x.Id.Value,
                x.Name,
                x.Code.Value,
                x.TaxCode ?? string.Empty,
                x.Email.Value,
                x.Phone != null ? x.Phone.Value : string.Empty,
                x.Address ?? string.Empty,
                x.CreatedAt
            ));

            var items = await _queryExecutor.ToListAsync(dtoQuery, cancellationToken);

            var result = new OffsetPagedResult<GetPageListCustomersResponse>
            {
                Items = items,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? 10,
                TotalCount = totalCount
            };

            return Result<OffsetPagedResult<GetPageListCustomersResponse>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<OffsetPagedResult<GetPageListCustomersResponse>>.Failure(
                $"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
        }
    }
}
