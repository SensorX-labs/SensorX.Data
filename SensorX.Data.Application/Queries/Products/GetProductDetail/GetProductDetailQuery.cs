using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetProductDetail;

public sealed record GetProductDetailQuery(
    Guid Id
) : IRequest<Result<GetProductDetailResponse>>;