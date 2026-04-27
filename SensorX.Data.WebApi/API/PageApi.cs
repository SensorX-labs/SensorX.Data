using MediatR;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Queries.Pages.GetPageProductDetail;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API;

public static class PageApi
{
    public static RouteGroupBuilder MapPageApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("pages").WithTags("Pages");

        api.MapGet("/product-detail/{id:guid}", GetPageProductDetail)
            .WithOpenApi()
            .WithSummary("Get product detail for product page")
            .WithDescription("Lấy thông tin chi tiết sản phẩm bao gồm cả thông tin đề xuất giá nội bộ để hiển thị trên trang chi tiết sản phẩm.");

        return api;
    }

    private static async Task<IResult> GetPageProductDetail(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetPageProductDetailQuery(id));
        return result.ToResult();
    }
}
