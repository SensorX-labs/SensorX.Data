using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.UnitOfQuantities.DeleteUnitOfQuantity;

public sealed class DeleteUnitOfQuantityHandler(
    IRepository<UnitOfQuantity> _unitRepository
) : IRequestHandler<DeleteUnitOfQuantityCommand, Result>
{
    public async Task<Result> Handle(DeleteUnitOfQuantityCommand request, CancellationToken cancellationToken)
    {
        var id = new UnitOfQuantityId(request.Id);
        var unit = await _unitRepository.GetByIdAsync(id, cancellationToken);
        if (unit is null)
            return Result.Failure("Không tìm thấy đơn vị tính");

        await _unitRepository.DeleteAsync(unit, cancellationToken);
        return Result.Success("Xóa đơn vị tính thành công.");
    }
}
