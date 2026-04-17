using System.Reflection;

namespace SensorX.Data.WebApi.Configurations
{
    public static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your services here

            // MediatR - scan từ Assembly Application
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.Load("SensorX.Data.Application"));
                cfg.LicenseKey = configuration["MediatR:LicenseKey"];
            });
            return services;
        }
    }
}

