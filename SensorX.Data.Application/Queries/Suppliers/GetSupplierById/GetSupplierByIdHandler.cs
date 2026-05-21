using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;

namespace SensorX.Data.Application.Queries.Suppliers.GetSupplierById;

public sealed class GetSupplierByIdHandler(
    IQueryBuilder<Supplier> _supplierQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<GetSupplierByIdQuery, Result<GetSupplierByIdResponse>>
{
    public async Task<Result<GetSupplierByIdResponse>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _supplierQueryBuilder.QueryAsNoTracking
            .Where(x => x.Id == request.Id)
            .Select(x => new GetSupplierByIdResponse(
                x.Id.Value,
                x.Name,
                x.Description,
                x.CreatedAt,
                x.UpdatedAt
            ));

        var item = await _queryExecutor.FirstOrDefaultAsync(query, cancellationToken);
        return item is null
            ? Result<GetSupplierByIdResponse>.Failure("Không tìm thấy nhà cung cấp")
            : Result<GetSupplierByIdResponse>.Success(item);
    }
}
