using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Suppliers.GetAllSuppliers;

public sealed record GetAllSuppliersQuery : IRequest<Result<List<GetAllSuppliersResponse>>>;
