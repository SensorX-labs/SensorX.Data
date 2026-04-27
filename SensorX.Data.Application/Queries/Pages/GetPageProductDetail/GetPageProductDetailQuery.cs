using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Pages.GetPageProductDetail;

public sealed record GetPageProductDetailQuery(
    Guid Id
) : IRequest<Result<GetPageProductDetailResponse>>;