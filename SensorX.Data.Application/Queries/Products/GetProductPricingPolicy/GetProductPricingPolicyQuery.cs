using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

public sealed record GetProductPricingPolicyQuery(List<Guid> ProductIds) : IRequest<Result<List<GetProductPricingPolicyResponse>>>;


