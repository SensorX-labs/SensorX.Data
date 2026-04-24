using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.ChangeProductStatus;
using SensorX.Data.Application.Commands.Products.CreateProduct;
using SensorX.Data.Application.Commands.Products.DeleteProduct;
using SensorX.Data.Application.Commands.Products.UpdateProduct;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Products.GetPageListProducts;
using SensorX.Data.Application.Queries.Products.GetProductDetail;
using SensorX.Data.Application.Queries.Products.GetProductPricingPolicy;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class ProductApi
{
    public static RouteGroupBuilder MapProductApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog").WithTags("Products");

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

        api.MapDelete("/products/{id:guid}", DeleteProduct)
            .WithOpenApi()
            .WithSummary("Delete product")
            .WithDescription("""
                - Id: Product ID from route to delete
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

        api.MapGet("/products/list", GetPageListProducts)
            .WithOpenApi()
            .WithSummary("Get page list products")
            .WithDescription("""
                - SearchTerm: Filter by name/description
                - PageNumber: The page number to retrieve (default: 1)
                - PageSize: Number of items per page (default: 10)
                """);

        api.MapPost("/products/pricing-policy/batch", GetProductPricingPolicy)
            .WithOpenApi()
            .WithSummary("Get product pricing policy")
            .WithDescription("""
                - ProductIds: List of product IDs
                """);

        api.MapGet("/products/{id:guid}", GetProductDetail)
            .WithOpenApi()
            .WithSummary("Get product detail");

        return api;
    }

    private static async Task<IResult> GetPageListProducts(
        [FromServices] IMediator mediator,
        [AsParameters] GetPageListProductsQuery query
    )
    {
        var result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> GetProductDetail(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        Result<GetProductDetailResponse> result = await mediator.Send(new GetProductDetailQuery(id));
        return result.ToResult();
    }

    private static async Task<IResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> DeleteProduct(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new DeleteProductCommand(id));
        return result.ToResult();
    }

    private static async Task<IResult> GetProductPricingPolicy(
        [FromBody] GetProductPricingPolicyQuery query,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(query);
        return result.ToResult();
    }

    private static async Task<IResult> UpdateProduct(
        [FromRoute] Guid id,
        [FromBody] UpdateProductCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }

    private static async Task<IResult> ChangeProductStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeProductStatusCommand command,
        [FromServices] IMediator mediator
    )
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToResult();
    }
}