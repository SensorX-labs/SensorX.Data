namespace SensorX.Data.Application.Queries.Products.GetProductListStats;

public sealed record ProductListStatsResponse(
    int TotalCount,
    int ActiveCount,
    int InactiveCount
);