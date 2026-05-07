using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Provinces.GetListProvince;

public sealed record GetListProvinceQuery() : IRequest<Result<List<GetListProvinceResponse>>>;