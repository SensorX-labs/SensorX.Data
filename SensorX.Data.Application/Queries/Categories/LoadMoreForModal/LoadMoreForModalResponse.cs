using SensorX.Data.Application.Common.QueryExtensions.LoadMore;

namespace SensorX.Data.Application.Queries.Categories.LoadMoreForModal;

public sealed record LoadMoreForModalResponse(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAt
);

public sealed class LoadMoreForModalResult : LoadMoreResult<LoadMoreForModalResponse>;