using SensorX.Data.Application.Common.ResponseClient;

namespace SensorX.Data.Application.Common.Interfaces;

public interface IVietnamAdministrativeService
{
    Task<Result<bool>> SyncAdministrativeDataAsync(CancellationToken cancellationToken = default);
}
