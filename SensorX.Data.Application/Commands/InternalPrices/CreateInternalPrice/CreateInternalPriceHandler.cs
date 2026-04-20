using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;

public class CreateInternalPriceHandler(
    IRepository<InternalPrice> _internalPriceRepository,
    IRepository<Product> _productRepository
) : IRequestHandler<CreateInternalPriceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateInternalPriceCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return Result<Guid>.Failure("Không tìm thấy sản phẩm");

        var suggestedPrice = Money.FromVnd(request.SuggestedPrice);
        var floorPrice = Money.FromVnd(request.FloorPrice);

        var internalPrice = InternalPrice.Create(
            productId,
            suggestedPrice,
            floorPrice,
            [.. request.PriceTiers.Select(t => new PriceTier(new Quantity(t.Quantity), Money.FromVnd(t.Price)))]
        );

        await _internalPriceRepository.AddAsync(internalPrice, cancellationToken);

        return Result<Guid>.Success(internalPrice.Id.Value);
    }
}