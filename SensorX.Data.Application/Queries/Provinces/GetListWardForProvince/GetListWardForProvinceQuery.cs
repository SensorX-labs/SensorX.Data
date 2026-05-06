using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Provinces.GetListWardForProvince;

public sealed record GetListWardForProvinceQuery(
    Guid ProvinceId
) : IRequest<Result<List<GetListWardForProvinceResponse>>>;
