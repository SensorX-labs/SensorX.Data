using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceSuggest;

public sealed record GetInternalPriceSuggestQuery(List<Guid> ProductIds)
    : IRequest<Result<List<InternalPriceResponse>>>;