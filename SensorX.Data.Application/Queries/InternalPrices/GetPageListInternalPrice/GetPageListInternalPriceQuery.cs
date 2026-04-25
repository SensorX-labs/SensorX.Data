using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;

public sealed record GetPageListInternalPriceQuery : OffsetPagedQuery, IRequest<Result<InternalPriceOffsetPagedResult>>
{
    public string? SearchTerm { get; init; }
    public InternalPriceStatus? Status { get; init; }
}