using MediatR;
using SensorX.Data.Application.Common.ResponseClient;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.SeedWork;

namespace SensorX.Data.Application.Queries.Products.GetPageListProducts;

public class GetPageListProductsHandler(
    IRepository<Product> _productRepository
) : IRequestHandler<GetPageListProductsQuery, Result<PaginatedResult<GetPageListProductsDto>>>
{
    public async Task<Result<PaginatedResult<GetPageListProductsDto>>> Handle(
        GetPageListProductsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Lấy tất cả sản phẩm từ repository
            var allProducts = await _productRepository.ListAsync(cancellationToken);

            // Chuyển đổi danh sách thành IQueryable để áp dụng LINQ
            var query = allProducts.AsQueryable();

            // Áp dụng bộ lọc tìm kiếm theo tên, mã sản phẩm hoặc hãng sản xuất
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.Value.ToLower().Contains(searchTerm) ||
                    p.Manufacture.ToLower().Contains(searchTerm)
                );
            }

            // Áp dụng bộ lọc danh mục nếu được chỉ định
            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId.Value == request.CategoryId.Value);
            }

            // Tính tổng số lượng sản phẩm phù hợp với bộ lọc
            var totalCount = query.Count();

            // Tính số dòng cần bỏ qua (skip) cho phân trang
            var skip = (request.PageNumber - 1) * request.PageSize;

            // Sắp xếp theo thời gian tạo (mới nhất trước) và áp dụng phân trang
            var products = query
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(request.PageSize)
                .ToList();

            // Chuyển đổi Product entity thành DTO để trả về API
            var productDtos = products.Select(p => new GetPageListProductsDto
            {
                Id = p.Id.Value,
                Code = p.Code.Value,
                Name = p.Name,
                Manufacture = p.Manufacture,
                Unit = p.Unit,
                Status = (int)p.Status,
                CategoryId = p.CategoryId.Value,
                CategoryName = null,
                
                // Chuyển Showcase (nếu có)
                Showcase = p.Showcase != null ? new ProductShowcaseDto
                {
                    Summary = p.Showcase.Summary,
                    Body = p.Showcase.Body
                } : null,
                
                // Chuyển danh sách ảnh sản phẩm
                Images = p.Images.Select(img => new ProductImageDto
                {
                    ImageUrl = img.ImageUrl
                }).ToList(),
                
                // Chuyển danh sách thuộc tính sản phẩm
                Attributes = p.Attributes.Select(attr => new ProductAttribute
                {
                    AttributeName = attr.AttributeName,
                    AttributeValue = attr.AttributeValue
                }).ToList(),
                
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            // Tạo kết quả phân trang với thông tin meta
            var paginatedResult = new PaginatedResult<GetPageListProductsDto>(
                productDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            // Trả về kết quả thành công
            return Result<PaginatedResult<GetPageListProductsDto>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            // Trả về lỗi nếu có exception
            return Result<PaginatedResult<GetPageListProductsDto>>.Failure($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}");
        }
    }
}
