using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Suppliers.GetSupplierById;

public sealed record GetSupplierByIdQuery(Guid Id) : IRequest<Result<GetSupplierByIdResponse>>;
