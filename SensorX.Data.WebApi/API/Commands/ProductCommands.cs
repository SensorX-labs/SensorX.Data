using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands.Products.ChangeProductStatus;
using SensorX.Data.Application.Commands.Products.CreateProduct;
using SensorX.Data.Application.Commands.Products.DeleteProduct;
using SensorX.Data.Application.Commands.Products.UpdateProduct;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Commands;

public static class ProductCommands
{
    public static RouteGroupBuilder MapProductCommands(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("catalog/products").WithTags("Product Commands");

        api.MapPost("/create", CreateProduct)
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

        api.MapDelete("/{id:guid}", DeleteProduct)
            .WithOpenApi()
            .WithSummary("Delete product")
            .WithDescription("""
                - Id: ID sản phẩm cần xóa từ route
                """);

        api.MapPut("/{id:guid}", UpdateProduct)
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

        api.MapPatch("/{id:guid}/status", ChangeProductStatus)
            .WithOpenApi()
            .WithSummary("Change product status")
            .WithDescription("""
                - Id: ID sản phẩm từ route
                - Status: Trạng thái mới (0: Đang hoạt động, 1: Ngừng hoạt động)
                """);

        return api;
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
