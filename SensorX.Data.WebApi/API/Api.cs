namespace SensorX.Data.WebApi.API;

public static class Api
{
    public static RouteGroupBuilder MapApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        api.MapCategoryApi();
        api.MapInternalPriceApi();
        api.MapProductApi();
        api.MapCustomerApi();
        api.MapStaffApi();
        api.MapImageApi();
        return api;
    }
}