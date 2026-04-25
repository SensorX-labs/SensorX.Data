using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;

public sealed class DeactivateInternalPriceHandler(
    IRepository<InternalPrice> _internalPriceRepository
) : IRequestHandler<DeactivateInternalPriceCommand, Result>
{
    public async Task<Result> Handle(DeactivateInternalPriceCommand request, CancellationToken cancellationToken)
    {
        var internalPrice = await _internalPriceRepository.GetByIdAsync(new InternalPriceId(request.Id), cancellationToken);
        if (internalPrice is null)
            return Result.Failure("Không tìm thấy bảng giá.");

        if (internalPrice.IsExpired())
            return Result.Failure("Bảng giá đã hết hạn.");

        internalPrice.MarkExpiredNow();
        await _internalPriceRepository.UpdateAsync(internalPrice, cancellationToken);

        return Result.Success("Đã hủy bảng giá thành công.");
    }
}