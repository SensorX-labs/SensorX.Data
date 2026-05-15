using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.ValueObjects;
using SensorX.Data.Infrastructure.DI;
using SensorX.Data.Infrastructure.Persistences;
using SensorX.Data.WebApi.API;
using SensorX.Data.WebApi.Configurations;
using SensorX.Data.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("Jwt:RequireHttpsMetadata");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false, // Thường Gateway đã validate rồi
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultHandler>();

builder.Services.AddServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    // Yêu cầu .NET tự động chuyển đổi giữa String và Enum
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen(options =>
{
    options.UseInlineDefinitionsForEnums();
    // Đăng ký Filter để hiển thị Enum dạng String trên Swagger UI
    options.SchemaFilter<SensorX.Data.WebApi.Filters.EnumSchemaFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandler();

var autoApplyMigration = builder.Configuration.GetValue("Migration:AutoApply", true);
if (autoApplyMigration)
{
    const int maxMigrationRetries = 12;
    for (var attempt = 1; attempt <= maxMigrationRetries; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Set<Category>().AnyAsync())
            {
                var cat1 = Category.Create("Thiết bị tự động hóa", "Cảm biến, PLC, Biến tần");
                var cat2 = Category.Create("Thiết bị đo lường", "Đồng hồ áp suất, lưu lượng");
                dbContext.Set<Category>().AddRange(cat1, cat2);
                await dbContext.SaveChangesAsync();

                var p1 = Product.Create(Code.From("PRD-001"), "Cảm biến tiệm cận Omron E2E", "Omron", cat1.Id, ProductStatus.Active, "Cái");
                var p2 = Product.Create(Code.From("PRD-002"), "Cảm biến quang Autonics BEN10M", "Autonics", cat1.Id, ProductStatus.Active, "Cái");
                var p3 = Product.Create(Code.From("PRD-003"), "Đồng hồ đo áp suất Wika 213.53", "Wika", cat2.Id, ProductStatus.Active, "Cái");
                dbContext.Set<Product>().AddRange(p1, p2, p3);
                await dbContext.SaveChangesAsync();
                app.Logger.LogInformation("Đã khởi tạo dữ liệu danh mục và sản phẩm mẫu thành công.");
            }
            break;
        }
        catch (Exception ex) when (attempt < maxMigrationRetries)
        {
            app.Logger.LogWarning(
                ex,
                "Data API migration attempt {Attempt}/{MaxRetries} failed. Retrying in 5 seconds...",
                attempt,
                maxMigrationRetries);
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserContextMiddleware>();
app.MapApi();

app.Run();
