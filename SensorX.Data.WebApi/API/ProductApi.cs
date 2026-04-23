using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.ChangeProductStatus;
using SensorX.Data.Application.Commands.Products.CreateProduct;
using SensorX.Data.Application.Commands.Products.DeleteProduct;
using SensorX.Data.Application.Commands.Products.UpdateProduct;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Products.GetPageListProducts;
using SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;

namespace SensorX.Data.WebApi.API;

public static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Products");

        api.MapGet("/products/list", GetPageListProducts)
            .WithOpenApi()
            .WithSummary("Get page list products")
            .WithDescription("""
                - SearchTerm: Filter by name/description
                - PageNumber: The page number to retrieve (default: 1)
                - PageSize: Number of items per page (default: 10)
                """);

        api.MapPost("/products/create", CreateProduct)
            .WithOpenApi()
            .WithSummary("Create product")
            .WithDescription("""
                - Name: Product name
                - Manufacture: Product manufacturer
                - CategoryId: Category unique identifier
                - Unit: Unit of measure (e.g., Kg, Pcs)
                - Showcase: Optional showcase link or name
                - ImageUrls: Optional list of image URLs
                - Attributes: Optional list of product attributes
                """);

        api.MapPost("/products/delete/{id:guid}", DeleteProduct)
            .WithOpenApi()
            .WithSummary("Delete product")
            .WithDescription("""
                - Id: Product ID from route to delete
                """);

        api.MapPost("/products/pricing-policy/batch", GetProductPricingPolicy)
            .WithOpenApi()
            .WithSummary("Get product pricing policy")
            .WithDescription("""
                - ProductIds: List of product IDs
                """);

        api.MapPut("/products/{id:guid}", UpdateProduct)
            .WithOpenApi()
            .WithSummary("Update product")
            .WithDescription("""
                - Id: Product ID from route
                - Name: New product name
                - Manufacture: New manufacturer
                - CategoryId: New category ID
                - Unit: New unit of measure
                - Showcase: Optional new showcase info
                - ImageUrls: Optional new image list
                - Attributes: Optional new attribute list
                """);

        api.MapPatch("/products/{id:guid}/status", ChangeProductStatus)
            .WithOpenApi()
            .WithSummary("Change product status")
            .WithDescription("""
                - Id: Product ID from route
                - Status: New status (0: Active, 1: Inactive)
                """);

        return api;
    }

    private static async Task<Results<Ok<Result<ProductOffsetPagedResult>>, BadRequest<string>>> GetPageListProducts(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListProductsQuery query
    )
    {
        var result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Lỗi khi lấy danh sách sản phẩm");
    }

    private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateProduct(
        [FromBody] CreateProductCommand command,
        [FromServices] IMediator mediator
    )
    {
        Result<Guid> result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Unknown error");
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
            : TypedResults.NotFound(result.Message ?? "Product not found");
    }
    private static async Task<Results<Ok<Result<List<GetProductPricingPolicyResponse>>>, BadRequest<string>>> GetProductPricingPolicy(
        [FromBody] GetProductPricingPolicyQuery query,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(query);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result.Message ?? "Lỗi khi lấy chính sách định giá");
    }
    private static async Task<Results<Ok<Result>, NotFound<string>>> UpdateProduct(
        [FromRoute] Guid id,
        [FromBody] UpdateProductCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result.Message ?? "Product not found");
    }
    private static async Task<Results<Ok<Result>, NotFound<string>>> ChangeProductStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeProductStatusCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        Result result = await mediator.Send(command);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.NotFound(result.Message ?? "Product not found");
    }
}