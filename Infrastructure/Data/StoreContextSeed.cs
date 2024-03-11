using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructue.Data;
using System.Text.Json;

namespace Infrastructure.Data;
public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.ProductBrands.Any())
        {
            var brandData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            context.ProductBrands.AddRange(brands);
        }

        if (!context.ProductTypes.Any())
        {
            var typeData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
            var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
            context.ProductTypes.AddRange(types);
        }

        if (!context.Products.Any())
        {
            var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
            var produtcts = JsonSerializer.Deserialize<List<Product>>(productData);
            context.Products.AddRange(produtcts);
        }

        if (!context.DeliveryMethods.Any())
        {
            var delivery = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
            var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(delivery);
            context.DeliveryMethods.AddRange(methods);
        }

        if (context.ChangeTracker.HasChanges())
            await context.SaveChangesAsync();
    }
}
