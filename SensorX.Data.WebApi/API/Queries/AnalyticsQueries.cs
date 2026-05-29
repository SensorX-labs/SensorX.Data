using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SensorX.Data.Application.Queries.Analytics.GetDashboardMasterStats;
using SensorX.Data.WebApi.Extensions;
using System.Threading.Tasks;

namespace SensorX.Data.WebApi.API.Queries;

public static class AnalyticsQueries
{
    public static RouteGroupBuilder MapAnalyticsQueries(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("analytics").WithTags("Analytics Queries");

        api.MapGet("master-stats", GetDashboardMasterStats).WithOpenApi(operation =>
        {
            operation.Summary = "Lấy thống kê dữ liệu hệ thống (Customers, Products, Staffs)";
            return operation;
        });

        return api;
    }

    private static async Task<IResult> GetDashboardMasterStats(
        [FromQuery] string? timeRange,
        [FromServices] IMediator mediator
    )
    {
        var result = await mediator.Send(new GetDashboardMasterStatsQuery(timeRange ?? "month"));
        return result.ToResult();
    }
}
