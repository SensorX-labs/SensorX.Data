# SensorX.Data Project Memory

## Pagination System (Updated 2026-04-23)

The project now supports two types of pagination in `SensorX.Data.Application.Common.QueryExtensions`:

### 1. Offset Pagination (`OffsetPagination` folder)
- **Use case**: When total page numbers and total item counts are required (e.g., standard web tables).
- **Base Query**: `OffsetPagedQuery` (contains `PageNumber`, `PageSize`).
- **Result Wrapper**: `OffsetPagedResult<T>` (contains `TotalCount`, `TotalPages`, `HasNextPage`, etc.).
- **Extension**: `ApplyOffsetPagination(request)`.
- **Implementation Note**: Requires a `CountAsync` call before applying pagination to get the total count.

### 2. Keyset Pagination (`KeysetPagination` folder) - Formerly Cursor Pagination
- **Use case**: High-performance infinite scroll or large datasets where `OFFSET` performance is an issue.
- **Base Query**: `KeysetPagedQuery` (contains `LastCreatedAt`, `LastId`, etc.).
- **Result Wrapper**: `KeysetPagedResult<T>` (contains cursors for next/previous).
- **Extension**: `ApplyKeysetPagination(request, createdAtSelector, idSelector)`.
- **Implementation Note**: 
    - Use `Take(PageSize + 1)` to determine `hasMore`.
    - `HasNext = IsPrevious ? true : hasMore`.
    - `HasPrevious = IsPrevious ? hasMore : (LastCreatedAt.HasValue || LastId.HasValue)`.

## Features Updated to Offset Pagination
- `GetPageListCategories`
- `GetPageListProducts`
- `GetPageListStaffs`
- `GetPageListCustomers`

## Internal Price Management (Updated 2026-04-24)

### Commands & API
- **CreateInternalPrice**: Creates a new price policy. Supports "Infinite" (no expiry) or fixed duration.
- **DeactivateInternalPrice**: `PATCH /api/catalog/internalPrices/{id}/deactivate`. Marks a price list as expired immediately.
- **ExtendInternalPrice**: `PATCH /api/catalog/internalPrices/{id}/extend`. Allows extending the expiration date or adding a duration.

### Validation Rules
- `SuggestedPrice` (Price for Qty 1) must be greater than or equal to `FloorPrice`.
- `PriceTiers` must have `Quantity > 1`.
- Price must decrease as Quantity increases across tiers.
### Suggestion Logic (Updated 2026-04-27)
- **Selection**: Returns the most "relevant" active price per product using `GroupBy(ProductId)` and `First()`.
- **Priority Rules**:
    1. `CreatedAt DESC`: Prefers more recently created price lists.
    2. `(ExpiresAt - CreatedAt) ASC`: Prefers "short-term" (temporary/promotional) price lists over "infinite" ones if they share similar creation times.
    3. `Id DESC`: Final deterministic fallback.

## Technical Patterns
- Use `IQueryBuilder<T>` to build the base query.
- Use `IQueryExecutor` for materialization (`ToListAsync`, `CountAsync`).
- **Grouping in Query**: Use `GroupBy(x => x.ProductId).Select(g => g.OrderBy(...).First())` to efficiently pick the best candidate per group in the database (supported in EF Core 6+).
- Always apply `OrderBy` before or as part of pagination logic to ensure deterministic results.
## Database & Migrations

### Recent Fixes (2026-05-06)
- **LINQ Translation Error in Administrative Queries**: Fixed an issue in `GetListProvinceHandler` and `GetListWardForProvinceHandler` where `.OrderBy(x => x.Code)` was called after `.Select(...)`.
    - **Cause**: EF Core cannot translate ordering logic based on client-side DTO properties after projection.
    - **Solution**: Moved `.OrderBy(x => x.Code)` before the `.Select(...)` projection to ensure the ordering happens at the database level using entity properties.
- **PendingModelChangesWarning**: Fixed an issue where the API failed to start due to `Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning`.
    - **Cause**: The `Department` property in `Staff` entity was changed to non-nullable in code, but the database schema hadn't been updated with a migration.
    - **Solution**: Created and applied a new migration `UpdateModelChanges` to sync the database schema with the model.
    - **EF Core 9+ Note**: EF Core now strictly validates that the model matches migrations on startup if automatic migrations are enabled. Always run `dotnet ef migrations add` after changing entities.

