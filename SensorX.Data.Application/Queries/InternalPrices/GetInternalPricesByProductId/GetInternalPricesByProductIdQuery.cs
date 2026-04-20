using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.InternalPrices.GetInternalPricesByProductId;

public class GetInternalPricesByProductIdQuery : IRequest<Result<GetInternalPricesByProductIdResponse>>
{
    public Guid ProductId { get; set; }

    public GetInternalPricesByProductIdQuery() { }

    public GetInternalPricesByProductIdQuery(Guid productId)
    {
        ProductId = productId;
    }
}
