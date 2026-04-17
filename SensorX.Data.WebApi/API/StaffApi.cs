using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SensorX.Data.Application.Commands;
using SensorX.Data.Application.Commands.Staffs.CreateStaff;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Application.Queries.Staffs.GetPageListStaffs;

namespace SensorX.Data.WebApi.API
{
    public static class StaffApi
    {
        public static RouteGroupBuilder MapStaffApi(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/staff").WithTags("Staff");

            api.MapPost("/create", CreateStaff).WithOpenApi();
            api.MapGet("/list", GetPageListStaffs).WithOpenApi();
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
    }
}
