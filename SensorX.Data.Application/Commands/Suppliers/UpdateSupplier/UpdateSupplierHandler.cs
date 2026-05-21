using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierHandler(
    IRepository<Supplier> _supplierRepository,
    IQueryBuilder<Supplier> _supplierQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<UpdateSupplierCommand, Result>
{
    public async Task<Result> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var id = new SupplierId(request.Id);
        var supplier = await _supplierRepository.GetByIdAsync(id, cancellationToken);
        if (supplier is null)
            return Result.Failure("Không tìm thấy nhà cung cấp");

        var normalizedName = request.Name.Trim().ToLower();
        var existingSupplier = await _queryExecutor.FirstOrDefaultAsync(
            _supplierQueryBuilder.QueryAsNoTracking
                .Where(x => x.Name.ToLower() == normalizedName && x.Id != request.Id)
                .Select(x => x.Id.Value),
            cancellationToken);

        if (existingSupplier != Guid.Empty)
            return Result.Failure("Nhà cung cấp đã tồn tại");

        supplier.Update(request.Name, request.Description ?? string.Empty);
        await _supplierRepository.UpdateAsync(supplier, cancellationToken);

        return Result.Success("Cập nhật nhà cung cấp thành công.");
    }
}
