using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsHandler(
    IQueryBuilder<Staff> _staffBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetPageListStaffsQuery, Result<StaffCursorPagedResult>>
{
    public async Task<Result<StaffCursorPagedResult>> Handle(
        GetPageListStaffsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Lấy query từ repository
            var query = _staffBuilder.QueryAsNoTracking;

            // Áp dụng bộ lọc tìm kiếm theo tên, mã hoặc email
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.Value.ToLower().Contains(searchTerm) ||
                    p.Email.Value.ToLower().Contains(searchTerm)
                );
            }

            // Áp dụng bộ lọc theo StaffId nếu được chỉ định
            if (request.StaffId.HasValue)
            {
                query = query.Where(p => p.Id.Value == request.StaffId.Value);
            }

            // Tính tổng số lượng nhân viên phù hợp với bộ lọc (không phân trang)
            var totalCount = await _queryExecutor.CountAsync(query, cancellationToken);

            // Tính số dòng cần bỏ qua (skip) cho phân trang
            var skip = (request.PageNumber - 1) * request.PageSize;

            // Sắp xếp và lấy dữ liệu phân trang từ Database
            var staffs = await _queryExecutor.ToListAsync(query
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(request.PageSize), cancellationToken);

            // Chuyển đổi Staff entity thành DTO để trả về API
            var staffDtos = staffs.Select(p => new GetPageListStaffsResponse
            {
                Id = p.Id.Value,
                AccountId = p.AccountId.Value.ToString(),
                Code = p.Code.Value,
                Name = p.Name,
                Phone = p.Phone.Value,
                Email = p.Email.Value,
                CitizenId = p.CitizenId.Value,
                Biography = p.Biography,
                JoinDate = p.JoinDate,
                Department = p.Department.ToString(),
                CreatedAt = p.CreatedAt
            }).ToList();

            // Tạo kết quả phân trang
            var paginatedResult = new PaginatedResult<GetPageListStaffsResponse>(
                staffDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            // Trả về kết quả thành công
            return Result<PaginatedResult<GetPageListStaffsResponse>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            // Trả về lỗi nếu có exception
            return Result<PaginatedResult<GetPageListStaffsResponse>>.Failure(
                $"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
        }
    }
}
