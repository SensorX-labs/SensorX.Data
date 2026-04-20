using SensorX.Data.WebApi.API;

namespace SensorX.Data.WebApi;

public static class Api
{
    public static RouteGroupBuilder MapApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        api.MapProductCategoryApi();
        api.MapInternalPriceApi();
        api.MapProductApi();
        api.MapCustomerApi();
        api.MapStaffApi();
        api.MapUploadApi();
        return api;
    }
}