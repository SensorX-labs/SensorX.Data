using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.SeedWork;
using SensorX.Data.Infrastructure.Persistences;

namespace SensorX.Data.Infrastructure.Services;

public class VietnamAdministrativeService(
    HttpClient _httpClient,
    IRepository<Province> _provinceRepository,
    ILogger<VietnamAdministrativeService> _logger
) : IVietnamAdministrativeService
{
    private const string ProvinceUrl = "https://provinces.open-api.vn/api/v2/p";
    private const string WardUrl = "https://provinces.open-api.vn/api/v2/w";

    public async Task<Result<bool>> SyncAdministrativeDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting sync of Vietnam administrative data...");

            var provincesDto = await _httpClient.GetFromJsonAsync<List<ProvinceDto>>(ProvinceUrl, cancellationToken);
            var wardsDto = await _httpClient.GetFromJsonAsync<List<WardDto>>(WardUrl, cancellationToken);


            if (provincesDto == null || wardsDto == null)
            {
                return Result<bool>.Failure("Failed to fetch data from API");
            }

            // 1. Tải dữ liệu hiện có từ Database để check trùng
            // Vì Ward là Owned Type nên nó sẽ được tự động Include khi dùng EF nếu cấu hình đúng, 
            // nhưng ở đây ta dùng .Include() cho chắc chắn nếu Repository hỗ trợ AsQueryable
            var existingProvinces = await _provinceRepository.AsQueryable()
                .Include(p => p.Wards)
                .ToDictionaryAsync(p => p.Code, cancellationToken);

            var wardsByProvince = wardsDto.ToLookup(w => w.ProvinceCode);
            int provincesAdded = 0;
            int wardsAdded = 0;

            foreach (var provinceDto in provincesDto)
            {
                // 2. Kiểm tra Tỉnh đã tồn tại chưa
                if (!existingProvinces.TryGetValue(provinceDto.Code, out var province))
                {
                    province = new Province(ProvinceId.New(), provinceDto.Name, provinceDto.Code);
                    await _provinceRepository.AddAsync(province, cancellationToken);
                    provincesAdded++;
                }

                // 3. Kiểm tra các Xã/Phường của tỉnh này
                var existingWardCodes = province.Wards.Select(w => w.Code).ToHashSet();


                foreach (var wardDto in wardsByProvince[provinceDto.Code])
                {
                    if (!existingWardCodes.Contains(wardDto.Code))
                    {
                        province.AddWard(wardDto.Name, wardDto.Code);
                        wardsAdded++;
                    }
                }
            }

            await _provinceRepository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Sync completed. Added {PCount} provinces and {WCount} wards.", provincesAdded, wardsAdded);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing Vietnam administrative data");
            return Result<bool>.Failure($"Sync failed: {ex.Message}");
        }
    }

    private class ProvinceDto
    {
        public string Name { get; set; } = string.Empty;
        public int Code { get; set; }
        [JsonPropertyName("division_type")]
        public string DivisionType { get; set; } = string.Empty;
        [JsonPropertyName("phone_code")]
        public int PhoneCode { get; set; }
        public string Codename { get; set; } = string.Empty;
    }

    private class WardDto
    {
        public string Name { get; set; } = string.Empty;
        public int Code { get; set; }
        [JsonPropertyName("division_type")]
        public string DivisionType { get; set; } = string.Empty;
        [JsonPropertyName("province_code")]
        public int ProvinceCode { get; set; }
        public string Codename { get; set; } = string.Empty;
    }
}
