using SensorX.Data.WebApi.API.Commands;
using SensorX.Data.WebApi.API.Queries;
using SensorX.Data.WebApi.API.Services;

namespace SensorX.Data.WebApi.API;

public static class Api
{
    public static RouteGroupBuilder MapApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api");

        //Command apis
        api.MapCustomerCommands();
        api.MapStaffCommands();
        api.MapCategoryCommands();
        api.MapProductCommands();
        api.MapInternalPriceCommands();
        api.MapSupplierCommands();
        api.MapUnitOfQuantityCommands();

        //Query apis
        api.MapCustomerQueries();
        api.MapStaffQueries();
        api.MapCategoryQueries();
        api.MapProductQueries();
        api.MapInternalPriceQueries();
        api.MapSupplierQueries();
        api.MapUnitOfQuantityQueries();

        //Service apis
        api.MapImageService();
        api.MapVietnamAdministrativeService();
        return api;
    }
}
