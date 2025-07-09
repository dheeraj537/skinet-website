using System;
using System.Text.Json;
using Core.Entitites;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Products.Any())
        {
            var productData = await File.ReadAllBytesAsync("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productData);
            if (products == null) return;

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
