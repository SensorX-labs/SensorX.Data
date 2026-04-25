using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetHistoryPriceForProduct;

public sealed record GetHistoryPriceForProductQuery([property: JsonIgnore] Guid ProductId) : IRequest<Result<GetHistoryPriceForProductResponse>>
{
}