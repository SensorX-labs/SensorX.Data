using MediatR;
using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Queries.Categories.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<Result<List<GetAllCategoriesResponse>>>;
