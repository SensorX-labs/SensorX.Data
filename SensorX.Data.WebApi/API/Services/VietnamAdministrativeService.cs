using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Provinces.GetListProvince;
using SensorX.Data.Application.Queries.Provinces.GetListWardForProvince;
using SensorX.Data.WebApi.Extensions;

namespace SensorX.Data.WebApi.API.Services;

public static class VietnamAdministrativeService
{
    public static RouteGroupBuilder MapVietnamAdministrativeService(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("vietnam-administrative").WithTags("Vietnam Administrative Services");

        api.MapPost("/sync", SyncVietnamAdministrativeData).WithOpenApi();
        api.MapGet("/getListProvince", GetListProvince).WithOpenApi();
        api.MapGet("/getListWardForProvince/{ProvinceId}", GetListWardForProvince).WithOpenApi();
        return api;
    }

    private static async Task<IResult> SyncVietnamAdministrativeData(
        [FromServices] IVietnamAdministrativeService vietnamAdministrativeService
    )
    {
        Result<bool> result = await vietnamAdministrativeService.SyncAdministrativeDataAsync();
        return result.ToResult();
    }

    private static async Task<IResult> GetListProvince(
        [FromServices] IMediator mediator
    )
    {
        Result<List<GetListProvinceResponse>> result = await mediator.Send(new GetListProvinceQuery());
        return result.ToResult();
    }

    private static async Task<IResult> GetListWardForProvince(
        [FromServices] IMediator mediator,
        [FromRoute] Guid ProvinceId
    )
    {
        Result<List<GetListWardForProvinceResponse>> result = await mediator.Send(new GetListWardForProvinceQuery(ProvinceId));
        return result.ToResult();
    }

}