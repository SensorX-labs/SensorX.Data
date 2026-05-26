using MediatR;
using SensorX.Data.Application.Common.Interfaces;
using SensorX.Data.Application.Common.QueryExtensions.OffsetPagination;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.SupplierAggregate;

namespace SensorX.Data.Application.Queries.Suppliers.GetPageListSuppliers;

public sealed class GetPageListSuppliersHandler(
    IQueryBuilder<Supplier> supplierQueryBuilder,
    IQueryExecutor queryExecutor
) : IRequestHandler<GetPageListSuppliersQuery, Result<OffsetPagedResult<GetPageListSuppliersResponse>>>
{
    public async Task<Result<OffsetPagedResult<GetPageListSuppliersResponse>>> Handle(
        GetPageListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<Supplier> query = supplierQueryBuilder.QueryAsNoTracking;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(term) ||
                    (x.Description != null && x.Description.ToLower().Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var nameTerm = request.Name.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(nameTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                var descriptionTerm = request.Description.Trim().ToLower();
                query = query.Where(x =>
                    x.Description != null && x.Description.ToLower().Contains(descriptionTerm));
            }

            if (request.HasDescription.HasValue)
            {
                query = request.HasDescription.Value
                    ? query.Where(x => !string.IsNullOrWhiteSpace(x.Description))
                    : query.Where(x => string.IsNullOrWhiteSpace(x.Description));
            }

            if (request.IsUpdated.HasValue)
            {
                query = request.IsUpdated.Value
                    ? query.Where(x => x.UpdatedAt.HasValue)
                    : query.Where(x => !x.UpdatedAt.HasValue);
            }

            if (request.CreatedFrom.HasValue)
            {
                var createdFrom = new DateTimeOffset(
                    request.CreatedFrom.Value.ToDateTime(TimeOnly.MinValue),
                    TimeSpan.Zero);
                query = query.Where(x => x.CreatedAt >= createdFrom);
            }

            if (request.CreatedTo.HasValue)
            {
                var createdToExclusive = new DateTimeOffset(
                    request.CreatedTo.Value.ToDateTime(TimeOnly.MinValue),
                    TimeSpan.Zero).AddDays(1);
                query = query.Where(x => x.CreatedAt < createdToExclusive);
            }

            var totalCount = await queryExecutor.CountAsync(query, cancellationToken);

            var items = await queryExecutor.ToListAsync(
                query.OrderByDescending(x => x.CreatedAt)
                    .ThenByDescending(x => x.Id)
                    .ApplyOffsetPagination(request)
                    .Select(x => new GetPageListSuppliersResponse(
                        x.Id.Value,
                        x.Name,
                        x.Description ?? string.Empty,
                        x.CreatedAt,
                        x.UpdatedAt
                    )),
                cancellationToken);

            return Result<OffsetPagedResult<GetPageListSuppliersResponse>>.Success(
                new OffsetPagedResult<GetPageListSuppliersResponse>
                {
                    Items = items,
                    PageNumber = request.PageNumber ?? 1,
                    PageSize = request.PageSize ?? 10,
                    TotalCount = totalCount
                });
        }
        catch (Exception ex)
        {
            return Result<OffsetPagedResult<GetPageListSuppliersResponse>>.Failure(
                $"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
        }
    }
}
