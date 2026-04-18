using MediatR;
using SensorX.Data.Application.Common.Dtos.Requests;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.InternalPrices.CreateInternalPrice;

public class CreateInternalPriceCommand : IRequest<Result<Guid>>
{
    public Guid ProductId { get; set; }
    public decimal SuggestedPrice { get; set; }
    public decimal FloorPrice { get; set; }
    public List<PriceTierRequest> PriceTiers { get; set; } = [];
}

