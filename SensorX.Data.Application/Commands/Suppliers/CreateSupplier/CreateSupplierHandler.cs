using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.Suppliers.CreateSupplier;

public sealed class CreateSupplierHandler(
    IRepository<Supplier> _supplierRepository,
    IQueryBuilder<Supplier> _supplierQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<CreateSupplierCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<Guid>.Failure("Tên nhà cung cấp không được để trống");

        var normalizedName = request.Name.Trim().ToLower();
        var existingSupplier = await _queryExecutor.FirstOrDefaultAsync(
            _supplierQueryBuilder.QueryAsNoTracking
                .Where(x => x.Name.ToLower() == normalizedName)
                .Select(x => x.Id.Value),
            cancellationToken);

        if (existingSupplier != Guid.Empty)
            return Result<Guid>.Failure("Nhà cung cấp đã tồn tại");

        var supplier = Supplier.Create(request.Name, request.Description ?? string.Empty);

        await _supplierRepository.AddAsync(supplier, cancellationToken);
        return Result<Guid>.Success(supplier.Id.Value, "Tạo nhà cung cấp thành công.");
    }
}
