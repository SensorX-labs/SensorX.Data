namespace SensorX.Data.Application.Queries.Provinces.GetListWardForProvince;

public sealed record GetListWardForProvinceResponse(
    Guid Id,
    int Code,
    string Name
);