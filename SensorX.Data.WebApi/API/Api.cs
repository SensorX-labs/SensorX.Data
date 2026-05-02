using SensorX.Data.WebApi.API.Commands;
using SensorX.Data.WebApi.API.Queries;

namespace SensorX.Data.WebApi.API;

public static class Api
{
    public static RouteGroupBuilder MapApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        //Command apis
        api.MapInternalPriceCommands();

        //Query apis
        api.MapCategoryApi();
        api.MapInternalPriceQueries();
        api.MapProductApi();
        api.MapCustomerApi();
        api.MapStaffApi();
        api.MapImageApi();
        api.MapPageApi();
        return api;
    }
}