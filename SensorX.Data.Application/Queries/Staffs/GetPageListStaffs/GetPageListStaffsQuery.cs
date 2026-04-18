using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

public class GetPageListStaffsQuery : IRequest<Result<PaginatedResult<GetPageListStaffsResponse>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? StaffId { get; set; }

    public GetPageListStaffsQuery() { }

    public GetPageListStaffsQuery(int pageNumber, int pageSize, string? searchTerm = null, Guid? staffId = null)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 && pageSize <= 100 ? pageSize : 10;
        SearchTerm = searchTerm;
        StaffId = staffId;
    }
}
