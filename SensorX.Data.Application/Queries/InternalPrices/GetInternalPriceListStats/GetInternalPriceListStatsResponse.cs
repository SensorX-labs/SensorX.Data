namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPriceListStats;

public sealed record GetInternalPriceListStatsResponse(
    int TotalCount,
    int ActiveCount,
    int ExpiringSoonCount,
    int ExpiredCount
);