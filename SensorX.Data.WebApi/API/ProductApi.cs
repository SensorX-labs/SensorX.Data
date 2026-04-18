using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.CreateProductCommand;
using SensorX.Data.Application.Commands.Products.DeleteProductCommand;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Products.GetPageListProducts;

namespace SensorX.Data.WebApi.API;

public static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Products");

        api.MapGet("/products", GetPageListProducts).WithOpenApi();
        api.MapPost("/products", CreateProduct).WithOpenApi();
        api.MapDelete("/products/{id:guid}", DeleteProduct).WithOpenApi();

        return api;
    }

    private static async Task<Results<Ok<Result<PaginatedResult<GetPageListProductsResponse>>>, BadRequest<string>>> GetPageListProducts(
        [FromServices] IMediator mediator,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? categoryId = null
    )
    {
        var query = new GetPageListProductsQuery(pageNumber, pageSize, searchTerm, categoryId);
        var result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy danh sách sản phẩm");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateProduct(
        [FromBody] CreateProductCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Unknown error");
    }

    private static async Task<Results<Ok<Result>, NotFound<string>>> DeleteProduct(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var command = new DeleteProductCommand { ProductId = id };
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result.Error ?? "Product not found");
    }
}
