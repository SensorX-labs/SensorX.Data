using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Common.Extensions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate.Specifications;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;

public sealed class CreateInternalPriceHandler(
    IRepository<InternalPrice> _internalPriceRepository,
    IRepository<Product> _productRepository
) : IRequestHandler<CreateInternalPriceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateInternalPriceCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return Result<Guid>.Failure("Không tìm thấy sản phẩm.");

        // Check if there's already an active infinite price for this product
        if (request.IsInfinite)
        {
            var spec = new ActiveInfiniteInternalPriceSpec(productId);
            var existingInfinitePrice = await _internalPriceRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (existingInfinitePrice != null)
            {
                if (!existingInfinitePrice.IsExpired())
                {
                    return Result<Guid>.Failure("Đã tồn tại bảng giá vô hạn cho sản phẩm này. Vui lòng xử lý bảng giá hiện tại trước.");
                }
            }
        }

        var suggestedPrice = Money.FromVnd(request.SuggestedPrice);
        var floorPrice = Money.FromVnd(request.FloorPrice);

        var internalPrice = InternalPrice.Create(
            productId,
            suggestedPrice,
            floorPrice,
            [.. request.PriceTiers.Select(t => new PriceTier(new Quantity(t.Quantity), Money.FromVnd(t.Price)))]
        );

        // Set expiration
        if (request.IsInfinite)
        {
            internalPrice.MarkInfinite();
        }
        else if (request.ExpiresAt.HasValue)
        {
            internalPrice.ExtendExpiryAt(request.ExpiresAt.Value);
        }
        else
        {
            internalPrice.MarkExpiredIn(TimeSpan.FromDays(30));
        }

        await _internalPriceRepository.AddAsync(internalPrice, cancellationToken);

        return Result<Guid>.Success(internalPrice.Id.Value);
    }
}