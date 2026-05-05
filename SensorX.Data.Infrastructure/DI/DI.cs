using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Infrastructure.Persistences;
using SensorX.Data.Infrastructure.Services;

namespace SensorX.Data.Infrastructure.DI
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddMassTransit(x =>
            {
                // Đăng ký Consumer
                x.AddConsumer<SensorX.Data.Application.Events.Consumers.CreateAccount.CreateAccountConsumer>();

                // Đăng ký Entity Framework Outbox
                x.AddEntityFrameworkOutbox<AppDbContext>(o =>
                {
                    // Sử dụng Postgres
                    o.UsePostgres();

                    // Quan trọng: Báo cho MassTransit biết hãy đóng vai trò là Outbox
                    o.UseBusOutbox();
                });

                // Cấu hình RabbitMQ
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqHost = configuration["RabbitMQ:Host"] ?? "localhost";
                    var rabbitMqPortStr = configuration["RabbitMQ:Port"];
                    var rabbitMqPort = ushort.TryParse(rabbitMqPortStr, out var port) ? port : (ushort)5672;
                    var rabbitMqVHost = configuration["RabbitMQ:VirtualHost"] ?? "/";

                    cfg.Host(rabbitMqHost, rabbitMqPort, rabbitMqVHost, h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    // Đổi tên Exchange giống với Gateway
                    cfg.Message<SensorX.Data.Application.Events.Consumers.CreateAccount.CreateAccountEvent>(e =>
                        e.SetEntityName("account-created"));

                    // Cấu hình Queue để consume event
                    cfg.ReceiveEndpoint("account-created-consumer", e =>
                    {
                        e.ConfigureConsumer<SensorX.Data.Application.Events.Consumers.CreateAccount.CreateAccountConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));
            services.AddScoped<IQueryExecutor, QueryExecutor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            // Cloudinary
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            return services;
        }
    }
}

