using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;
using SensorX.Data.Application.Queries.Staffs.GetStaffMetrics;

namespace SensorX.Data.WebApi.API
{
    public static class StaffApi
    {
        public static RouteGroupBuilder MapStaffApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("staff").WithTags("Staff");

            api.MapPost("/create", CreateStaff).WithOpenApi();
            api.MapGet("/list", GetPageListStaffs).WithOpenApi();
            api.MapGet("/{staffId:guid}/metrics", GetEmployeeMetrics).WithOpenApi();
            return api;
        }

        private static async Task<Results<Ok<Result<Guid>>, BadRequest<string>>> CreateStaff(
            [FromBody] CreateStaffCommand command,
            [FromServices] IMediator mediator
        )
        {
            Result<Guid> result = await mediator.Send(command);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result.Error);
        }

        private static async
            Task<Results<Ok<Result<PaginatedResult<GetPageListStaffsResponse>>>, BadRequest<string>>>
            GetPageListStaffs(
                [FromServices] IMediator mediator,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? searchTerm = null,
                [FromQuery] Guid? staffId = null
            )
        {
            var query = new GetPageListStaffsQuery(pageNumber, pageSize, searchTerm, staffId);
            var result = await mediator.Send(query);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy danh sách nhân viên");
        }

        private static async Task<Results<Ok<Result<GetStaffMetricsResponse>>, BadRequest<string>>> GetEmployeeMetrics(
            [FromRoute] Guid staffId,
            [FromServices] IMediator mediator
        )
        {
            var query = new GetStaffMetricsQuery(staffId);
            var result = await mediator.Send(query);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result.Error ?? "Lỗi khi lấy chỉ số hiệu suất");
        }
    }
}
