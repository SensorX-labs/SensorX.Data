using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;

public record CreateInternalPriceCommand(
    Guid ProductId,
    decimal SuggestedPrice,
    decimal FloorPrice,
    List<PriceTierDto> PriceTiers
) : IRequest<Result<Guid>>
{
    public List<PriceTierDto> PriceTiers { get; init; } = PriceTiers ?? [];
}
public record PriceTierDto(int Quantity, decimal Price);
