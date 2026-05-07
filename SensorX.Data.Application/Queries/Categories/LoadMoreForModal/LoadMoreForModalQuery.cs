using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.LoadMore;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Categories.LoadMoreForModal;

public sealed record LoadMoreForModalQuery : LoadMoreQuery, IRequest<Result<LoadMoreForModalResult>>
{
    public string? SearchTerm { get; init; }
}