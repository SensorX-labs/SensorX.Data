using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Application.Queries.UnitOfQuantities.GetUnitOfQuantityById;

public sealed class GetUnitOfQuantityByIdHandler(
    IQueryBuilder<UnitOfQuantity> _unitQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetUnitOfQuantityByIdQuery, Result<GetUnitOfQuantityByIdResponse>>
{
    public async Task<Result<GetUnitOfQuantityByIdResponse>> Handle(GetUnitOfQuantityByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _unitQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == request.Id)
            .Select(x => new GetUnitOfQuantityByIdResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var item = await _queryExecutor.FirstOrDefaultAsync(query, cancellationToken);
        return item is null
            ? Result<GetUnitOfQuantityByIdResponse>.Failure("Không tìm thấy đơn vị tính")
            : Result<GetUnitOfQuantityByIdResponse>.Success(item);
    }
}
