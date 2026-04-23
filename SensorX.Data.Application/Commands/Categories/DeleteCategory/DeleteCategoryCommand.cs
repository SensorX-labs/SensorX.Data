using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.DeleteCategory;

public sealed record DeleteCategoryCommand([property: JsonIgnore] Guid Id) : IRequest<Result>;
