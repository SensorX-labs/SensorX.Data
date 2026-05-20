using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.CreateUnitOfQuantity;

public sealed class CreateUnitOfQuantityHandler(
    IRepository<UnitOfQuantity> _unitRepository,
    IQueryBuilder<UnitOfQuantity> _unitQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<CreateUnitOfQuantityCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUnitOfQuantityCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<Guid>.Failure("Tên đơn vị tính không được để trống");

        var normalizedName = request.Name.Trim().ToLower();
        var existingUnit = await _queryExecutor.FirstOrDefaultAsync(
            _unitQueryBuilder.QueryAsNoTracking
                .Where(x => x.Name.ToLower() == normalizedName)
                .Select(x => x.Id.Value),
            cancellationToken);

        if (existingUnit != Guid.Empty)
            return Result<Guid>.Failure("Đơn vị tính đã tồn tại");

        var unit = UnitOfQuantity.Create(request.Name, request.Description ?? string.Empty);

        await _unitRepository.AddAsync(unit, cancellationToken);
        return Result<Guid>.Success(unit.Id.Value, "Tạo đơn vị tính thành công.");
    }
}
