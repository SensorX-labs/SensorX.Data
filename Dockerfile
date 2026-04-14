FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SensorX.Data.WebApi/SensorX.Data.WebApi.csproj", "SensorX.Data.WebApi/"]
COPY ["SensorX.Data.Infrastructure/SensorX.Data.Infrastructure.csproj", "SensorX.Data.Infrastructure/"]
COPY ["SensorX.Data.Application/SensorX.Data.Application.csproj", "SensorX.Data.Application/"]
COPY ["SensorX.Data.Domain/SensorX.Data.Domain.csproj", "SensorX.Data.Domain/"]

RUN dotnet restore "SensorX.Data.WebApi/SensorX.Data.WebApi.csproj"

COPY . .
RUN dotnet publish "SensorX.Data.WebApi/SensorX.Data.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SensorX.Data.WebApi.dll"]