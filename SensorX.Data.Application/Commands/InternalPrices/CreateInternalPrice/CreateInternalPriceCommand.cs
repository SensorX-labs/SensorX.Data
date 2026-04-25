using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;

public sealed record CreateInternalPriceCommand(
    Guid ProductId,
    decimal SuggestedPrice,
    decimal FloorPrice,
    List<PriceTierDto> PriceTiers,
    bool IsInfinite = false,
    DateTimeOffset? ExpiresAt = null
) : IRequest<Result<Guid>>;

public sealed record PriceTierDto(int Quantity, decimal Price);
