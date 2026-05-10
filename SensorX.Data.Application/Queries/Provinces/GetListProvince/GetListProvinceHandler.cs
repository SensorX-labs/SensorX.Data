using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Application.Queries.Provinces.GetListProvince;

public sealed class GetListProvinceHandler(
    IQueryBuilder<Province> _provinceBuilder,
    IQueryExecutor _queryExcutor
) : IRequestHandler<GetListProvinceQuery, Result<List<GetListProvinceResponse>>>
{
    public async Task<Result<List<GetListProvinceResponse>>> Handle(GetListProvinceQuery request, CancellationToken cancellationToken)
    {
        var query = _provinceBuilder.QueryAsNoTracking
            .OrderBy(x => x.Code)
            .Select(x => new GetListProvinceResponse(x.Id.Value, x.Code, x.Name));

        var result = await _queryExcutor.ToListAsync(query, cancellationToken);

        return Result<List<GetListProvinceResponse>>.Success(result);
    }
}