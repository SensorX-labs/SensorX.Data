using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SensorX.Data.Application.Queries.Analytics.GetDashboardMasterStats;

public class GetDashboardMasterStatsHandler(
    IQueryBuilder<Customer> customerQueryBuilder,
    IQueryBuilder<Product> productQueryBuilder,
    IQueryBuilder<Staff> staffQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetDashboardMasterStatsQuery, Result<GetDashboardMasterStatsResponse>>
{
    public async Task<Result<GetDashboardMasterStatsResponse>> Handle(
        GetDashboardMasterStatsQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var totalCustomers = await queryExecutor.CountAsync(customerQueryBuilder.QueryAsNoTracking, cancellationToken);
            var totalProducts = await queryExecutor.CountAsync(productQueryBuilder.QueryAsNoTracking.Where(p => p.Status == ProductStatus.Active), cancellationToken);
            var totalStaffs = await queryExecutor.CountAsync(staffQueryBuilder.QueryAsNoTracking.Where(s => s.Status == StaffStatus.Active), cancellationToken);

            // Use Vietnam timezone (UTC+7) to determine calendar boundaries
            var vnOffset = TimeSpan.FromHours(7);
            var vnNow = DateTimeOffset.UtcNow.ToOffset(vnOffset);
            DateTimeOffset? startDate = null;
            DateTimeOffset? prevStartDate = null;
            DateTimeOffset? prevEndDate = null;

            switch ((request.TimeRange ?? "month").ToLower())
            {
                case "today":
                    startDate = new DateTimeOffset(vnNow.Year, vnNow.Month, vnNow.Day, 0, 0, 0, vnOffset);
                    prevStartDate = startDate.Value.AddDays(-1);
                    prevEndDate = prevStartDate.Value.Add(vnNow - startDate.Value);
                    break;
                case "week":
                    int diff = (7 + (vnNow.DayOfWeek - DayOfWeek.Monday)) % 7;
                    var startOfWeek = vnNow.AddDays(-diff).Date;
                    startDate = new DateTimeOffset(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, vnOffset);
                    prevStartDate = startDate.Value.AddDays(-7);
                    prevEndDate = prevStartDate.Value.Add(vnNow - startDate.Value);
                    break;
                case "month":
                    startDate = new DateTimeOffset(vnNow.Year, vnNow.Month, 1, 0, 0, 0, vnOffset);
                    prevStartDate = startDate.Value.AddMonths(-1);
                    prevEndDate = prevStartDate.Value.Add(vnNow - startDate.Value);
                    break;
                case "year":
                    startDate = new DateTimeOffset(vnNow.Year, 1, 1, 0, 0, 0, vnOffset);
                    prevStartDate = startDate.Value.AddYears(-1);
                    prevEndDate = prevStartDate.Value.Add(vnNow - startDate.Value);
                    break;
                case "all":
                default:
                    break;
            }

            int newCustomers;
            int prevNewCustomers;

            if (startDate.HasValue)
            {
                newCustomers = await queryExecutor.CountAsync(
                    customerQueryBuilder.QueryAsNoTracking.Where(c => c.CreatedAt >= startDate.Value && c.CreatedAt <= vnNow),
                    cancellationToken
                );
            }
            else
            {
                newCustomers = await queryExecutor.CountAsync(customerQueryBuilder.QueryAsNoTracking, cancellationToken);
            }

            if (prevStartDate.HasValue && prevEndDate.HasValue)
            {
                prevNewCustomers = await queryExecutor.CountAsync(
                    customerQueryBuilder.QueryAsNoTracking.Where(c => c.CreatedAt >= prevStartDate.Value && c.CreatedAt < prevEndDate.Value),
                    cancellationToken
                );
            }
            else
            {
                prevNewCustomers = 0;
            }

            var response = new GetDashboardMasterStatsResponse
            {
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalStaffs = totalStaffs,
                NewCustomers = newCustomers,
                PreviousNewCustomers = prevNewCustomers
            };

            return Result<GetDashboardMasterStatsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetDashboardMasterStatsResponse>.Failure($"Loi khi thong ke master stats: {ex.Message}");
        }
    }
}
