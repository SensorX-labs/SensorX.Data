using MediatR;
using Microsoft.EntityFrameworkCore;
using SensorX.Data.Application.Common.Interfaces; // Giả sử interface nằm đây
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate; // Thêm để lấy Ward/Province
using SensorX.Data.Domain.StrongIDs;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersHandler(
    IReadRepository<Customer> _customerRepo
) : IRequestHandler<GetPageListCustomersQuery, Result<PaginatedResult<GetPageListCustomersResponse>>>
{
    public async Task<Result<PaginatedResult<GetPageListCustomersResponse>>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // 1. Tạo query cơ sở từ Customer (Sử dụng QueryAsNoTracking để tối ưu)
            var customerQuery = _customerRepo.QueryAsNoTracking;

            // 2. Áp dụng bộ lọc (Filter) trên bảng chính trước khi Join để tối ưu hiệu năng
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                customerQuery = customerQuery.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.Value.ToLower().Contains(searchTerm) ||
                    p.Email.Value.ToLower().Contains(searchTerm)
                );
            }

            if (request.CustomerId.HasValue)
            {
                var targetId = new CustomerId(request.CustomerId.Value);
                customerQuery = customerQuery.Where(p => p.Id == targetId);



                // 4. Tính tổng số lượng
                var totalCount = await joinedQuery.CountAsync(cancellationToken);

                // 5. Phân trang và Projection trực tiếp xuống Database
                var skip = (request.PageNumber - 1) * request.PageSize;

                var items = await joinedQuery
                    .OrderByDescending(x => x.c.CreatedAt)
                    .Skip(skip)
                    .Take(request.PageSize)
                    .Select(x => new GetPageListCustomersResponse
                    {
                        Id = x.c.Id.Value,
                        Name = x.c.Name,
                        Email = x.c.Email.Value,
                        PhoneNumber = x.c.Phone.Value,
                        TaxCode = x.c.TaxCode,
                        Address = x.c.Address,
                        ShippingInfo = x.c.ShippingInfo != null ? new ShippingInfoDto
                        {
                            WardId = x.c.ShippingInfo!.WardId.Value.ToString(),
                            WardName = x.w!.Name,
                            ProvinceId = x.w.ProvinceId.Value.ToString(),
                            ProvinceName = x.p!.Name,
                            RecipientName = x.c.ShippingInfo.ReceiverName,
                            RecipientPhone = x.c.ShippingInfo.ReceiverPhone.Value!,
                            RecipientAddress = x.c.ShippingInfo.ShippingAddress
                        } : null
                    })
                    .ToListAsync(cancellationToken);

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