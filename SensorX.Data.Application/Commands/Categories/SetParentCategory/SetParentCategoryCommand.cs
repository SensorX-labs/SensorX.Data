using System.Text.Json.Serialization;
using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Commands.Categories.SetParentCategory;

public sealed record SetParentCategoryCommand(
    [property: JsonIgnore] Guid Id,
    Guid? ParentId
) : IRequest<Result>;
