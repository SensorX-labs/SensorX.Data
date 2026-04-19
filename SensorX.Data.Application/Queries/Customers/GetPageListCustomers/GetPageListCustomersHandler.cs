using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersHandler(
    IQueryBuilder<Customer> _customerBuilder,
    IQueryBuilder<Ward> _wardBuilder,
    IQueryBuilder<Province> _provinceBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListCustomersQuery, Result<PaginatedResult<GetPageListCustomersResponse>>>
{
    public async Task<Result<PaginatedResult<GetPageListCustomersResponse>>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var customerQuery = _customerBuilder.QueryAsNoTracking;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                customerQuery = customerQuery.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.Value.ToLower().Contains(searchTerm) ||
                    p.Email.Value.ToLower().Contains(searchTerm)
                );
            }

            // JOIN logic to include Ward and Province names
            var joinedQuery = from c in customerQuery
                              join w in _wardBuilder.QueryAsNoTracking on c.ShippingInfo.WardId equals w.Id into wards
                              from w in wards.DefaultIfEmpty()
                              join p in _provinceBuilder.QueryAsNoTracking on w.ProvinceId equals p.Id into provinces
                              from p in provinces.DefaultIfEmpty()
                              select new { c, w, p };

            var totalCount = await _queryExecutor.CountAsync(joinedQuery, cancellationToken);

            var skip = (request.PageNumber - 1) * request.PageSize;

            var items = await _queryExecutor.ToListAsync(joinedQuery
                .OrderByDescending(x => x.c.CreatedAt)
                .Skip(skip)
                .Take(request.PageSize)
                .Select(x => new GetPageListCustomersResponse
                {
                    Id = x.c.Id.Value,
                    Name = x.c.Name,
                    Code = x.c.Code.Value,
                    Email = x.c.Email.Value,
                    PhoneNumber = x.c.Phone.Value,
                    TaxCode = x.c.TaxCode,
                    Address = x.c.Address,
                    ShippingInfo = x.c.ShippingInfo != null ? new ShippingInfoDto
                    {
                        WardId = x.c.ShippingInfo!.WardId.Value.ToString(),
                        WardName = x.w != null ? x.w.Name : null,
                        ProvinceId = x.w != null ? x.w.ProvinceId.Value.ToString() : null,
                        ProvinceName = x.p != null ? x.p.Name : null,
                        RecipientName = x.c.ShippingInfo.ReceiverName,
                        RecipientPhone = x.c.ShippingInfo.ReceiverPhone.Value!,
                        RecipientAddress = x.c.ShippingInfo.ShippingAddress
                    } : null
                }), cancellationToken);

            var paginatedResult = new PaginatedResult<GetPageListCustomersResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            return Result<PaginatedResult<GetPageListCustomersResponse>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<GetPageListCustomersResponse>>.Failure(
                $"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
        }
    }
}