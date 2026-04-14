using MediatR;
using SensorX.Data.Application.Common.Dtos;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.CreateInternalPrice;

public class CreateInternalPriceCommand : IRequest<Result<Guid>>
{
    public Guid ProductId { get; set; }
    public decimal SuggestedPrice { get; set; }
    public decimal FloorPrice { get; set; }
    public List<PriceTierDto> PriceTiers { get; set; } = [];
}

