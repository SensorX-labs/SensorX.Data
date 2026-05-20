using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.UpdateUnitOfQuantity;

public sealed class UpdateUnitOfQuantityHandler(
    IRepository<UnitOfQuantity> _unitRepository,
    IQueryBuilder<UnitOfQuantity> _unitQueryBuilder,
    IQueryExecutor _queryExecutor
) : IRequestHandler<UpdateUnitOfQuantityCommand, Result>
{
    public async Task<Result> Handle(UpdateUnitOfQuantityCommand request, CancellationToken cancellationToken)
    {
        var id = new UnitOfQuantityId(request.Id);
        var unit = await _unitRepository.GetByIdAsync(id, cancellationToken);
        if (unit is null)
            return Result.Failure("Không tìm thấy đơn vị tính");

        var normalizedName = request.Name.Trim().ToLower();
        var existingUnit = await _queryExecutor.FirstOrDefaultAsync(
            _unitQueryBuilder.QueryAsNoTracking
                .Where(x => x.Name.ToLower() == normalizedName && x.Id != request.Id)
                .Select(x => x.Id.Value),
            cancellationToken);

        if (existingUnit != Guid.Empty)
            return Result.Failure("Đơn vị tính đã tồn tại");

        unit.Update(request.Name, request.Description ?? string.Empty);
        await _unitRepository.UpdateAsync(unit, cancellationToken);

        return Result.Success("Cập nhật đơn vị tính thành công.");
    }
}
