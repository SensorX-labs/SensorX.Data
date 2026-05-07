using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Application.Queries.Provinces.GetListWardForProvince;

public sealed class GetListWardForProvinceHandler(
    IQueryBuilder<Province> _provinceBuilder,
    IQueryExecutor _queryExcutor
) : IRequestHandler<GetListWardForProvinceQuery, Result<List<GetListWardForProvinceResponse>>>
{
    public async Task<Result<List<GetListWardForProvinceResponse>>> Handle(GetListWardForProvinceQuery request, CancellationToken cancellationToken)
    {
        var query = _provinceBuilder.QueryAsNoTracking
            .Where(p => p.Id == new ProvinceId(request.ProvinceId))
            .SelectMany(p => p.Wards)
            .OrderBy(w => w.Code)
            .Select(w => new GetListWardForProvinceResponse(w.Id.Value, w.Code, w.Name));

        var result = await _queryExcutor.ToListAsync(query, cancellationToken);

        return Result<List<GetListWardForProvinceResponse>>.Success(result);
    }
}