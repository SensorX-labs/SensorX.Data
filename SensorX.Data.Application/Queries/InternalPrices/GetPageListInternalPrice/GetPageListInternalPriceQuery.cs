using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetPageListInternalPrice;

public sealed record GetPageListInternalPriceQuery : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListInternalPriceResponse>>>
{
    public string? SearchTerm { get; init; }
    public string? ProductCode { get; init; }
    public string? ProductName { get; init; }
    public InternalPriceStatus? Status { get; init; }
    public DateOnly? ExpiresFrom { get; init; }
    public DateOnly? ExpiresTo { get; init; }
    public decimal? SuggestedPriceFrom { get; init; }
    public decimal? SuggestedPriceTo { get; init; }
    public decimal? FloorPriceFrom { get; init; }
    public decimal? FloorPriceTo { get; init; }
}
