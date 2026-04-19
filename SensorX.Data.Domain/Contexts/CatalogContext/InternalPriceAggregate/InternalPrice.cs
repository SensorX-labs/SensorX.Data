using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public class InternalPrice : Entity<InternalPriceId>, IAggregateRoot, ICreationTrackable
{
    private InternalPrice(
        InternalPriceId id,
        ProductId productId
    ) : base(id)
    {
        ProductId = productId;
        SuggestedPrice = Money.Zero();
        FloorPrice = Money.Zero();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static InternalPrice Create(ProductId productId, Money suggestedPrice, Money floorPrice, IReadOnlyList<PriceTier> priceTiers)
    {
        var internalPrice = new InternalPrice(InternalPriceId.New(), productId);
        internalPrice.UpdateBasePrices(suggestedPrice, floorPrice);
        foreach (var priceTier in priceTiers)
        {
            internalPrice.AddPriceTier(priceTier);
        }
        return internalPrice;
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
    public bool CheckPrice(Money price, Quantity quantity)
    {
        //TODO: lặp qua các price tier để kiểm tra giá có hợp lệ với số lượng tương ứng không
        if (price < FloorPrice)
            throw new DomainException("Giá không được thấp hơn giá sàn.");
        if (price > SuggestedPrice)
            throw new DomainException("Giá không được cao hơn giá đề xuất.");
        var effectivePrice = GetEffectivePrice(quantity);
        if (price < effectivePrice.Price)
            throw new DomainException("Giá không được thấp hơn giá hiệu quả.");
        return true;
    }
}