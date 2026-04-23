using SensorX.Data.WebApi.API.CategoryApi;
using SensorX.Data.WebApi.API.CustomerApi;
using SensorX.Data.WebApi.API.InternalPriceApi;
using SensorX.Data.WebApi.API.ProductApi;
using SensorX.Data.WebApi.API.StaffApi;
using SensorX.Data.WebApi.API.UploadApi;

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
        api.MapUploadApi();
        return api;
    }
}