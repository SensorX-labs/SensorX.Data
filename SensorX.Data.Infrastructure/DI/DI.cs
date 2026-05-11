using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Infrastructure.Jobs;
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

            services.AddServices();
            services.AddMassTransit(configuration);
            services.AddQuartzJob();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IQueryBuilder<>), typeof(QueryBuilder<>));

            services.AddScoped<IQueryExecutor, QueryExecutor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddHttpClient<IVietnamAdministrativeService, VietnamAdministrativeService>();

            return services;
        }

        private static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                // Thêm chữ "Data" làm tiền tố cho Queue.
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("data", false));

                // Đăng ký Consumer
                x.AddConsumer<SensorX.Data.Application.Events.Consumers.CreateAccountEvent.CreateStaffConsumer>();
                x.AddConsumer<SensorX.Data.Application.Events.Consumers.CustomerRegisterAccountEvent.CreateCustomerConsumer>();

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

                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }

        private static IServiceCollection AddQuartzJob(this IServiceCollection services)
        {
            // Đăng ký Quartz
            services.AddQuartz(q =>
            {
                // Tạo JobKey
                var jobKey = new JobKey(nameof(ProcessDomainEventsJob));

                // Đăng ký Job vào DI container của Quartz
                q.AddJob<ProcessDomainEventsJob>(opts => opts.WithIdentity(jobKey));

                // Lên lịch (Trigger) cho Job chạy lặp đi lặp lại
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{nameof(ProcessDomainEventsJob)}-trigger")
                    // Chạy mỗi 5 giây, mãi mãi
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                );
            });

            // Chạy Quartz dưới dạng Hosted Service
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}

