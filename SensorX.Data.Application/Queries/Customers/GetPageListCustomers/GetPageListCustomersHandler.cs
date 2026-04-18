using MediatR;
using Microsoft.EntityFrameworkCore;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Customers.GetPageListCustomers;

public class GetPageListCustomersHandler(
    IRepository<Customer> _customerRepository
) : IRequestHandler<GetPageListCustomersQuery, Result<PaginatedResult<GetPageListCustomersResponse>>>
{
    public async Task<Result<PaginatedResult<GetPageListCustomersResponse>>> Handle(
        GetPageListCustomersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Lấy query từ repository với các Include để nạp dữ liệu liên quan
            var query = _customerRepository.AsQueryable()
                .Include(p => p.ShippingInfo.Ward)
                .ThenInclude(w => w.Province)
                .AsNoTracking();

            // Áp dụng bộ lọc tìm kiếm theo tên, mã khách hàng hoặc email
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.Value.ToLower().Contains(searchTerm) ||
                    p.Email.Value.ToLower().Contains(searchTerm)
                );
            }

            // Áp dụng bộ lọc khách hàng nếu được chỉ định
            if (request.CustomerId.HasValue)
            {
                query = query.Where(p => p.Id.Value == request.CustomerId.Value);
            }

            // Tính tổng số lượng khách hàng phù hợp với bộ lọc (không phân trang)
            var totalCount = await query.CountAsync(cancellationToken);

            // Tính số dòng cần bỏ qua (skip) cho phân trang
            var skip = (request.PageNumber - 1) * request.PageSize;

            // Sắp xếp và lấy dữ liệu phân trang từ Database
            var customers = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Chuyển đổi Customer entity thành DTO để trả về API
            var customerDtos = customers.Select(p => new GetPageListCustomersResponse
            {
                Id = p.Id.Value,
                Name = p.Name,
                Email = p.Email.Value,
                PhoneNumber = p.Phone.Value,
                TaxCode = p.TaxCode,
                Address = p.Address,
                ShippingInfo = p.ShippingInfo != null
                    ? new ShippingInfoDto
                    {
                        WardId = p.ShippingInfo.WardId.Value.ToString(),
                        WardName = p.ShippingInfo.Ward?.Name ?? "N/A",
                        ProvinceId = p.ShippingInfo.Ward?.ProvinceId.Value.ToString() ?? "N/A",
                        ProvinceName = p.ShippingInfo.Ward?.Province?.Name ?? "N/A",
                        RecipientName = p.ShippingInfo.ReceiverName,
                        RecipientPhone = p.ShippingInfo.ReceiverPhone?.Value ?? "N/A",
                        RecipientAddress = p.ShippingInfo.ShippingAddress
                    }
                    : null!
            }).ToList();

            // Tạo kết quả phân trang với thông tin meta
            var paginatedResult = new PaginatedResult<GetPageListCustomersResponse>(
                customerDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            // Trả về kết quả thành công
            return Result<PaginatedResult<GetPageListCustomersResponse>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            // Trả về lỗi nếu có exception
            return Result<PaginatedResult<GetPageListCustomersResponse>>.Failure(
                $"Lỗi khi lấy danh sách khách hàng: {ex.Message}");
        }
    }
}
