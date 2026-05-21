using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Suppliers.DeleteSupplier;

public sealed class DeleteSupplierHandler(
    IRepository<Supplier> _supplierRepository
) : IRequestHandler<DeleteSupplierCommand, Result>
{
    public async Task<Result> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var id = new SupplierId(request.Id);
        var supplier = await _supplierRepository.GetByIdAsync(id, cancellationToken);
        if (supplier is null)
            return Result.Failure("Không tìm thấy nhà cung cấp");

        await _supplierRepository.DeleteAsync(supplier, cancellationToken);
        return Result.Success("Xóa nhà cung cấp thành công.");
    }
}
