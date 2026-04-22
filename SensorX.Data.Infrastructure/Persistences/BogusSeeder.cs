using Bogus;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;
using SensorX.Data.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace SensorX.Data.Infrastructure.Persistences;

public static class BogusSeeder
{
    public static async Task SeedData(AppDbContext context)
    {
        await SeedStaffs(context);
        await SeedCustomers(context);
        await SeedCategories(context);
        await SeedProducts(context);
        await SeedInternalPrices(context);
    }

    private static async Task SeedStaffs(AppDbContext context)
    {
        if (await context.Set<Staff>().AnyAsync()) return;

        var staffFaker = new Faker<Staff>("vi")
            .CustomInstantiator(f =>
            {
                var staffId = StaffId.New();
                var accountId = new AccountId(Guid.NewGuid());
                var name = f.Name.FullName();
                var phone = Phone.From(f.PickRandom("09", "03", "08") + f.Random.Number(10000000, 99999999).ToString());
                var email = Email.From(f.Internet.Email());
                var code = Code.From("STF-" + DateTime.UtcNow.ToString("yyMMdd") + "-" + f.Random.Number(100000000, 999999999).ToString());
                var citizenId = CitizenId.From(f.Random.Number(100000, 999999).ToString() + f.Random.Number(100000, 999999).ToString());
                var biography = f.Lorem.Paragraph();
                var joinDate = f.Date.PastOffset(5).ToUniversalTime();
                var department = f.PickRandom<Department>();

                return new Staff(staffId, accountId, code, name, phone, email, citizenId, biography, joinDate, department);
            });

        await context.Set<Staff>().AddRangeAsync(staffFaker.Generate(10));
        await context.SaveChangesAsync();
    }

    private static async Task SeedCustomers(AppDbContext context)
    {
        if (await context.Set<Customer>().AnyAsync()) return;

        var customerFaker = new Faker<Customer>("vi")
            .CustomInstantiator(f =>
            {
                var customerId = CustomerId.New();
                var accountId = new AccountId(Guid.NewGuid());
                var name = f.Company.CompanyName();
                var phone = Phone.From(f.PickRandom("09", "03", "08") + f.Random.Number(10000000, 99999999).ToString());
                var email = Email.From(f.Internet.Email());
                var code = Code.From("CUS-" + DateTime.UtcNow.ToString("yyMMdd") + "-" + f.Random.Number(100000000, 999999999).ToString());
                var taxCode = f.Random.Number(100000000, 999999999).ToString();
                var address = f.Address.FullAddress();

                return new Customer(customerId, accountId, code, name, phone, email, taxCode, address);
            });

        await context.Set<Customer>().AddRangeAsync(customerFaker.Generate(20));
        await context.SaveChangesAsync();
    }

    private static async Task SeedCategories(AppDbContext context)
    {
        if (await context.Set<Category>().AnyAsync()) return;

        var categories = new List<Category>
        {
            Category.Create("Cảm biến", "Các loại cảm biến công nghiệp"),
            Category.Create("Thiết bị đo", "Thiết bị đo lường chính xác"),
            Category.Create("Tự động hóa", "Giải pháp tự động hóa dây chuyền"),
            Category.Create("Phụ kiện", "Linh kiện và phụ kiện đi kèm")
        };

        await context.Set<Category>().AddRangeAsync(categories);
        await context.SaveChangesAsync();

        var subCategories = new List<Category>
        {
            Category.Create("Cảm biến tiệm cận", "Cảm biến tiệm cận điện dung/điện cảm"),
            Category.Create("Cảm biến quang", "Cảm biến quang học"),
            Category.Create("Đồng hồ đo áp suất", "Đồng hồ đo áp suất thủy lực/khí nén")
        };

        subCategories[0].SetParent(categories[0]);
        subCategories[1].SetParent(categories[0]);
        subCategories[2].SetParent(categories[1]);

        await context.Set<Category>().AddRangeAsync(subCategories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProducts(AppDbContext context)
    {
        if (await context.Set<Product>().AnyAsync()) return;

        var categories = await context.Set<Category>().ToListAsync();
        
        var productFaker = new Faker<Product>("vi")
            .CustomInstantiator(f =>
            {
                var code = Code.From("PRD-" + DateTime.UtcNow.ToString("yyMMdd") + "-" + f.Random.Number(100000000, 999999999).ToString());
                var name = f.Commerce.ProductName();
                var manufacture = f.Company.CompanyName();
                var category = f.PickRandom(categories);
                var unit = f.PickRandom("Cái", "Bộ", "Chiếc", "Mét");

                return Product.Create(code, name, manufacture, category.Id, ProductStatus.Active, unit);
            });

        await context.Set<Product>().AddRangeAsync(productFaker.Generate(40));
        await context.SaveChangesAsync();
    }

    private static async Task SeedInternalPrices(AppDbContext context)
    {
        if (await context.Set<InternalPrice>().AnyAsync()) return;

        var products = await context.Set<Product>().ToListAsync();
        var faker = new Faker("vi");

        foreach (var product in products)
        {
            var basePrice = faker.Random.Number(1000, 50000) * 1000; // Giá từ 1tr đến 50tr
            
            var priceTiers = new List<PriceTier>
            {
                new(new Quantity(1), Money.FromVnd(basePrice)),
                new(new Quantity(10), Money.FromVnd(basePrice * 0.9m)),
                new(new Quantity(50), Money.FromVnd(basePrice * 0.85m))
            };

            var internalPrice = InternalPrice.Create(
                product.Id, 
                Money.FromVnd(basePrice), 
                Money.FromVnd(basePrice * 0.7m), 
                priceTiers);

            await context.Set<InternalPrice>().AddAsync(internalPrice);
        }

        await context.SaveChangesAsync();
    }
}
