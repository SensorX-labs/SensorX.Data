using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.InternalPrices.DeactivateInternalPrice;

public sealed record DeactivateInternalPriceCommand(
    Guid Id
) : IRequest<Result>;