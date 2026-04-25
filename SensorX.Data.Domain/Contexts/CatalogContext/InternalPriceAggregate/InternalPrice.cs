using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;

public class InternalPrice : Entity<InternalPriceId>, IAggregateRoot, ICreationTrackable, IExpirable
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
    public DateTimeOffset ExpiresAt { get; set; }

    public void AddPriceTier(PriceTier priceTier)
    {
        if (priceTier.Price < FloorPrice)
            throw new DomainException("Giá của bậc không được thấp hơn giá sàn.");

        if (priceTier.Price >= SuggestedPrice)
            throw new DomainException("Giá của bậc phải thấp hơn giá đề xuất (giá áp dụng cho số lượng 1).");

        if (priceTier.Quantity.Value <= 1)
            throw new DomainException("Các bậc giá phải áp dụng cho số lượng lớn hơn 1.");

        _priceTiers.Add(priceTier);
        ValidatePriceTiers();
    }

    public void UpdateBasePrices(Money suggested, Money floor)
    {
        if (suggested < floor)
            throw new DomainException("Giá đề xuất không được thấp hơn giá sàn.");

        SuggestedPrice = suggested;
        FloorPrice = floor;

        ValidatePriceTiers();
    }

    private void ValidatePriceTiers()
    {
        if (_priceTiers.Count == 0) return;
        var sortedTiers = _priceTiers.OrderBy(t => t.Quantity.Value).ToList();

        for (int i = 0; i < sortedTiers.Count; i++)
        {
            var current = sortedTiers[i];

            if (current.Price >= SuggestedPrice)
                throw new DomainException($"Giá bậc cho số lượng {current.Quantity.Value} phải thấp hơn giá đề xuất ({SuggestedPrice}).");

            if (current.Price < FloorPrice)
                throw new DomainException($"Giá bậc cho số lượng {current.Quantity.Value} không được thấp hơn giá sàn ({FloorPrice}).");

            if (i > 0)
            {
                var previous = sortedTiers[i - 1];
                if (current.Price >= previous.Price)
                {
                    throw new DomainException($"Giá phải giảm dần khi số lượng tăng lên. Giá bậc cho số lượng {current.Quantity.Value} ({current.Price}) phải thấp hơn giá bậc cho số lượng {previous.Quantity.Value} ({previous.Price}).");
                }
            }
        }
    }

    public PriceTier GetEffectivePrice(Quantity quantity)
    {
        if (quantity.Value <= 1)
            return new PriceTier(new Quantity(1), SuggestedPrice);

        var applicableTier = _priceTiers
            .Where(t => t.Quantity.Value <= quantity.Value)
            .OrderByDescending(t => t.Quantity.Value)
            .FirstOrDefault();

        return applicableTier ?? new PriceTier(quantity, SuggestedPrice);
    }

    public bool CheckPrice(Money price, Quantity quantity)
    {
        var effectivePrice = GetEffectivePrice(quantity);
        return price >= FloorPrice && price <= effectivePrice.Price;
    }
}