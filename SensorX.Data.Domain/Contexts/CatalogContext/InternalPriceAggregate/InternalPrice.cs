using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public class InternalPrice : Entity<InternalPriceId>, IAggregateRoot, ICreationTrackable
{
    private InternalPrice() { }
    public InternalPrice(
        InternalPriceId id,
        ProductId productId,
        Money suggestedPrice,
        Money floorPrice
    ) : base(id)
    {
        ProductId = productId;
        SuggestedPrice = suggestedPrice;
        FloorPrice = floorPrice;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public ProductId ProductId { get; private set; }
    public Money SuggestedPrice { get; private set; }
    public Money FloorPrice { get; private set; }
    private readonly List<PriceTier> _priceTiers = [];
    public IReadOnlyList<PriceTier> PriceTiers => _priceTiers.AsReadOnly();

    public DateTimeOffset CreatedAt { get; set; }

    public void AddPriceTier(PriceTier priceTier)
    {
        if (priceTier.Price < FloorPrice)
            throw new DomainException("Giá không được thấp hơn giá sàn.");
        if (priceTier.Price > SuggestedPrice)
            throw new DomainException("Giá không được cao hơn giá đề xuất.");
        _priceTiers.Add(priceTier);
    }
    public void UpdateBasePrices(Money suggested, Money floor)
    {
        if (suggested < floor)
            throw new DomainException("Giá đề xuất không được thấp hơn giá sàn.");
        SuggestedPrice = suggested;
        FloorPrice = floor;
    }

    public PriceTier GetEffectivePrice(Quantity quantity)
    {
        //TODO: lặp qua các price tier để tìm giá hiệu quả với số lượng tương ứng
        return _priceTiers.OrderByDescending(t => t.Quantity.Value).FirstOrDefault(t => t.Quantity.Value <= quantity.Value) ?? new PriceTier(quantity, FloorPrice);
    }
    public void CheckPrice(Money price, Quantity quantity)
    {
        //TODO: lặp qua các price tier để kiểm tra giá có hợp lệ với số lượng tương ứng không
    }
}