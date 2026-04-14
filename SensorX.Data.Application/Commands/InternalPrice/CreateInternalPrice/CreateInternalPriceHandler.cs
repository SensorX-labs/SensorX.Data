using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;


namespace SensorX.Data.Application.Commands.CreateInternalPrice;
public class CreateInternalPriceHandler(
    IRepository<InternalPrice> _internalPriceRepository,
    IRepository<Product> _productRepository,
    IUnitOfWork _unitOfWork
) : IRequestHandler<CreateInternalPriceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateInternalPriceCommand request, CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            return Result<Guid>.Failure("Không tìm thấy sản phẩm");
        }

        var suggestedPrice = Money.FromVnd(request.SuggestedPrice);
        var floorPrice = Money.FromVnd(request.FloorPrice);

        var internalPrice = new InternalPrice(
            InternalPriceId.New(),
            productId,
            suggestedPrice,
            floorPrice
        );

        // thêm price tiers
        foreach (var tierDto in request.PriceTiers)
        {
            var tier = new PriceTier(
                new Quantity(tierDto.Quantity),
                Money.FromVnd(tierDto.Price)
            );
            internalPrice.AddPriceTier(tier);
        }

        await _internalPriceRepository.AddAsync(internalPrice, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(internalPrice.Id.Value);
    }
}