using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Products.GetWarehouseProductContext;

public record GetWarehouseProductContextQuery : IRequest<Result<List<WarehouseProductContextDto>>>;
