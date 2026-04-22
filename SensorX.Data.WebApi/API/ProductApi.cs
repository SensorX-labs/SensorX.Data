using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.CreateProductCommand;
using SensorX.Data.Application.Commands.Products.DeleteProductCommand;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Products.GetPageListProducts;
using SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

namespace SensorX.Data.WebApi.API;

public static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Products");

        api.MapGet("/products", GetPageListProducts).WithOpenApi();
        api.MapPost("/products", CreateProduct).WithOpenApi();
        api.MapDelete("/products/{id:guid}", DeleteProduct).WithOpenApi();
        api.MapPost("/products/pricing-policy/batch", GetProductPricingPolicy).WithOpenApi();

        return api;
    }

    private static async Task<Results<Ok<Result<ProductCursorPagedResult>>, BadRequest<string>>> GetPageListProducts(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListProductsQuery query
    )
    {
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
        var command = new DeleteProductCommand(id);
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result.Error ?? "Product not found");
    }
    private static async Task<Results<Ok<Result<List<GetProductPricingPolicyResponse>>>, BadRequest<string>>> GetProductPricingPolicy(
        [FromBody] GetProductPricingPolicyRequest request,
        [FromServices] IMediator mediator
    )
    {
        var query = new GetProductPricingPolicyQuery(request.ProductIds);
        var result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy chính sách định giá");
    }
}

public class GetProductPricingPolicyRequest
{
    public List<Guid> ProductIds { get; set; } = [];
}
