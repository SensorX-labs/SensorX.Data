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
            var totalProducts = await queryExecutor.CountAsync(productQueryBuilder.QueryAsNoTracking, cancellationToken);
            var totalStaffs = await queryExecutor.CountAsync(staffQueryBuilder.QueryAsNoTracking, cancellationToken);

            var response = new GetDashboardMasterStatsResponse
            {
                TotalCustomers = totalCustomers,
                TotalProducts = totalProducts,
                TotalStaffs = totalStaffs
            };

            return Result<GetDashboardMasterStatsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<GetDashboardMasterStatsResponse>.Failure($"Loi khi thong ke master stats: {ex.Message}");
        }
    }
}
