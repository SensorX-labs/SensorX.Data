using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;

public sealed class ExtendInternalPriceHandler(
    IRepository<InternalPrice> _internalPriceRepository
) : IRequestHandler<ExtendInternalPriceCommand, Result>
{
    public async Task<Result> Handle(ExtendInternalPriceCommand request, CancellationToken cancellationToken)
    {
        var internalPrice = await _internalPriceRepository.GetByIdAsync(new InternalPriceId(request.Id), cancellationToken);
        if (internalPrice is null)
            return Result.Failure("Không tìm thấy bảng giá.");

        if (request.ExpiresAt.HasValue)
        {
            internalPrice.ExtendExpiryAt(request.ExpiresAt.Value);
        }
        else if (request.Duration.HasValue)
        {
            internalPrice.ExtendExpiry(request.Duration.Value);
        }
        else
        {
            return Result.Failure("Vui lòng cung cấp thời gian kết thúc hoặc thời gian gia hạn.");
        }
        await _internalPriceRepository.UpdateAsync(internalPrice, cancellationToken);

        return Result.Success("Gia hạn bảng giá thành công.");
    }
}