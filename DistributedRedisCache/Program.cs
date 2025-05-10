using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Redis connection
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

// Register CacheService for DI
builder.Services.AddScoped<CacheService>();

// Register logging services
builder.Services.AddLogging();

var app = builder.Build();

app.MapGet("/", async context =>
{
    var cacheService = context.RequestServices.GetRequiredService<CacheService>();
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    string cacheKey = "ProductList";
    string? cachedData = await cacheService.GetCacheAsync(cacheKey);

    if (!string.IsNullOrEmpty(cachedData))
    {
        logger.LogInformation("Cache Hit: " + cachedData);
        await context.Response.WriteAsync($"Cache Hit: {cachedData}");
    }
    else
    {
        logger.LogInformation("Cache Miss: Generating new data...");
        string productData = "Product1, Product2, Product3";

        await cacheService.SetCacheAsync(cacheKey, productData, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2));
        logger.LogInformation("Data Stored in Cache: " + productData);
        await context.Response.WriteAsync($"Cache Miss: {productData} - Stored in Cache");
    }
});

// Add an endpoint to manually invalidate the cache
app.MapGet("/invalidate-cache", async context =>
{
    var cacheService = context.RequestServices.GetRequiredService<CacheService>();
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    string cacheKey = "ProductList";

    logger.LogInformation("Cache invalidation triggered.");
    await cacheService.InvalidateCacheAsync(cacheKey);
    await context.Response.WriteAsync("Cache invalidated.");
});

await app.RunAsync();
