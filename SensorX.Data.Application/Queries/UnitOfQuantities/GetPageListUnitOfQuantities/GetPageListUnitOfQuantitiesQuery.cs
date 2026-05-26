using MediatR;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetPageListUnitOfQuantities;

public sealed record GetPageListUnitOfQuantitiesQuery
    : OffsetPagedQuery, IRequest<Result<OffsetPagedResult<GetPageListUnitOfQuantitiesResponse>>>
{
    public string? SearchTerm { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? HasDescription { get; init; }
    public bool? IsUpdated { get; init; }
    public DateOnly? CreatedFrom { get; init; }
    public DateOnly? CreatedTo { get; init; }
}
