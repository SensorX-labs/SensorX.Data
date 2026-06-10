namespace SensorX.Data.Application.Queries.Products.GetWarehouseProductContext;

public record WarehouseProductContextDto(
    Guid ProductId,
    string CategoryName,
    decimal CurrentPrice
);
