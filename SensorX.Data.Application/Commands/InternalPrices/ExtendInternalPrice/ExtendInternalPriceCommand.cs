using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Commands.InternalPrices.ExtendInternalPrice;

public sealed record ExtendInternalPriceCommand(
    [property: JsonIgnore] Guid Id,
    DateTimeOffset? ExpiresAt
) : IRequest<Result>;