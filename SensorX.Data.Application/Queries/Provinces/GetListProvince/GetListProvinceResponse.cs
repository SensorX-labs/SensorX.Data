namespace SensorX.Data.Application.Queries.Provinces.GetListProvince;

public sealed record GetListProvinceResponse(
    Guid Id,
    int Code,
    string Name
);