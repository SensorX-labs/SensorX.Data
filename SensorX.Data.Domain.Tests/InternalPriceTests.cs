using SensorX.Data.Domain.Common.Exceptions;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.ValueObjects;
using Xunit;

namespace SensorX.Data.Domain.Tests;

public class InternalPriceTests
{
    private readonly ProductId _productId = ProductId.New();
    private readonly Money _suggestedPrice = Money.FromVnd(100000);
    private readonly Money _floorPrice = Money.FromVnd(80000);

    [Fact]
    public void CreateInternalPrice_ShouldInitializeCorrectly()
    {
        // Act
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);

        // Assert
        Assert.NotNull(internalPrice.Id);
        Assert.Equal(_productId, internalPrice.ProductId);
        Assert.Equal(_suggestedPrice, internalPrice.SuggestedPrice);
        Assert.Equal(_floorPrice, internalPrice.FloorPrice);
    }

    [Fact]
    public void AddPriceTier_WhenPriceLowerThanFloor_ShouldThrowDomainException()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);
        var cheapPrice = Money.FromVnd(70000);
        var tier = new PriceTier(new Quantity(10), cheapPrice);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => internalPrice.AddPriceTier(tier));
        Assert.Equal("Giá của bậc không được thấp hơn giá sàn.", exception.Message);
    }

    [Fact]
    public void GetEffectivePrice_ShouldReturnHighestTierMatchingQuantity()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);

        internalPrice.AddPriceTier(new PriceTier(new Quantity(10), Money.FromVnd(95000)));
        internalPrice.AddPriceTier(new PriceTier(new Quantity(50), Money.FromVnd(90000)));
        internalPrice.AddPriceTier(new PriceTier(new Quantity(100), Money.FromVnd(85000)));

        // Act
        var priceFor5 = internalPrice.GetEffectivePrice(new Quantity(5));
        var priceFor15 = internalPrice.GetEffectivePrice(new Quantity(15));
        var priceFor60 = internalPrice.GetEffectivePrice(new Quantity(60));
        var priceFor150 = internalPrice.GetEffectivePrice(new Quantity(150));

        // Assert
        Assert.Equal(_suggestedPrice, priceFor5.Price); // No tier matches, return suggested price
        Assert.Equal(Money.FromVnd(95000), priceFor15.Price);
        Assert.Equal(Money.FromVnd(90000), priceFor60.Price);
        Assert.Equal(Money.FromVnd(85000), priceFor150.Price);
    }

    [Fact]
    public void UpdateBasePrices_WhenSuggestedLowerThanFloor_ShouldThrowDomainException()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);

        // Act & Assert
        Assert.Throws<DomainException>(() => internalPrice.UpdateBasePrices(Money.FromVnd(50000), Money.FromVnd(60000)));
    }

    [Fact]
    public void AddPriceTier_WhenPriceHigherThanSuggested_ShouldThrowDomainException()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);
        var expensivePrice = Money.FromVnd(110000);
        var tier = new PriceTier(new Quantity(10), expensivePrice);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => internalPrice.AddPriceTier(tier));
        Assert.Contains("phải thấp hơn giá đề xuất", exception.Message);
    }

    [Fact]
    public void AddPriceTier_WhenQuantityIsOneOrLess_ShouldThrowDomainException()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);
        var tier = new PriceTier(new Quantity(1), Money.FromVnd(90000));

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => internalPrice.AddPriceTier(tier));
        Assert.Equal("Các bậc giá phải áp dụng cho số lượng lớn hơn 1.", exception.Message);
    }

    [Fact]
    public void AddPriceTier_WhenPriceDoesNotDecreaseWithQuantity_ShouldThrowDomainException()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);
        internalPrice.AddPriceTier(new PriceTier(new Quantity(10), Money.FromVnd(90000)));

        var tier = new PriceTier(new Quantity(20), Money.FromVnd(95000)); // Higher price for higher quantity

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => internalPrice.AddPriceTier(tier));
        Assert.Contains("Giá phải giảm dần khi số lượng tăng lên", exception.Message);
    }

    [Fact]
    public void CheckPrice_ShouldReturnTrue_WhenPriceIsWithinBounds()
    {
        // Arrange
        var internalPrice = InternalPrice.Create(_productId, _suggestedPrice, _floorPrice, []);
        internalPrice.AddPriceTier(new PriceTier(new Quantity(10), Money.FromVnd(90000)));

        // Act & Assert
        Assert.True(internalPrice.CheckPrice(Money.FromVnd(95000), new Quantity(5))); // <= Suggested (100k)
        Assert.True(internalPrice.CheckPrice(Money.FromVnd(85000), new Quantity(15))); // <= Tier (90k) and >= Floor (80k)
        Assert.False(internalPrice.CheckPrice(Money.FromVnd(75000), new Quantity(15))); // < Floor (80k)
        Assert.False(internalPrice.CheckPrice(Money.FromVnd(95000), new Quantity(15))); // > Tier (90k)
    }
}
