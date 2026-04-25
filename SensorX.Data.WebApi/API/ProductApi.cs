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
                - Name: Tên sản phẩm
                - Manufacture: Nhà sản xuất sản phẩm
                - CategoryId: ID danh mục
                - Unit: Đơn vị tính (vd: Kg, Cái)
                - Showcase: Liên kết hoặc tên trưng bày tùy chọn
                - ImageUrls: Danh sách URL hình ảnh tùy chọn
                - Attributes: Danh sách thuộc tính sản phẩm tùy chọn
                """);

        api.MapDelete("/products/{id:guid}", DeleteProduct)
            .WithOpenApi()
            .WithSummary("Delete product")
            .WithDescription("""
                - Id: ID sản phẩm cần xóa từ route
                """);

        api.MapPut("/products/{id:guid}", UpdateProduct)
            .WithOpenApi()
            .WithSummary("Update product")
            .WithDescription("""
                - Id: ID sản phẩm từ route
                - Name: Tên sản phẩm mới
                - Manufacture: Nhà sản xuất mới
                - CategoryId: ID danh mục mới
                - Unit: Đơn vị tính mới
                - Showcase: Thông tin trưng bày mới tùy chọn
                - ImageUrls: Danh sách hình ảnh mới tùy chọn
                - Attributes: Danh sách thuộc tính mới tùy chọn
                """);

        api.MapPatch("/products/{id:guid}/status", ChangeProductStatus)
            .WithOpenApi()
            .WithSummary("Change product status")
            .WithDescription("""
                - Id: ID sản phẩm từ route
                - Status: Trạng thái mới (0: Đang hoạt động, 1: Ngừng hoạt động)
                """);

        api.MapGet("/products/list", GetPageListProducts)
            .WithOpenApi()
            .WithSummary("Get page list products")
            .WithDescription("""
                - SearchTerm: Lọc theo tên/mô tả
                - PageNumber: Số trang để lấy (mặc định: 1)
                - PageSize: Số lượng mục trên mỗi trang (mặc định: 10)
                """);

        api.MapPost("/products/pricing-policy/batch", GetProductPricingPolicy)
            .WithOpenApi()
            .WithSummary("Get product pricing policy")
            .WithDescription("""
                - ProductIds: Danh sách ID sản phẩm
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