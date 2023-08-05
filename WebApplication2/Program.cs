using WebApplication2.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using WebApplication2.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

string connectionString = builder.Configuration.GetConnectionString("WebAppContextConnection");

builder.Services.AddDbContext<WebAppDBContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    await SeedData.Initialize(services);
}

// add endpoints, dont forget to add migration, update database and check for correct data and structure in azure

app.MapGet("/laptops/search", (WebAppDBContext db, decimal? minPrice, decimal? maxPrice, Guid? stockInStore, Province? stockInProvince, LaptopCondition? condition, Guid? brandId, string? searchPhrase) => 
{
    try
    {
        List<Laptop> laptops = db.Laptops
            .Include(l => l.Brand)
            .Include(l => l.StoreLaptopStocks)
            .ThenInclude(sls => sls.Store).ToList();

        if (minPrice.HasValue)
        {
            if (minPrice < 0) { throw new ArgumentOutOfRangeException(nameof(minPrice), "Cannot search for negative prices"); }

            laptops = laptops.Where(l => l.Price >= minPrice).ToList();

            CheckIfEmpty("No laptops are over the specified minPrice");
        }

        if (maxPrice.HasValue)
        {
            if (maxPrice > decimal.MaxValue) { throw new ArgumentOutOfRangeException(nameof(minPrice), "Price excedes maximum value"); }

            laptops = laptops.Where(l => l.Price <= maxPrice).ToList();

            CheckIfEmpty("No laptops are under the speficied maxPrice");
        }

        if (stockInStore.HasValue)
        {
            if (!db.Stores.Any(s => s.StoreNumber == stockInStore))
            {
                throw new ArgumentOutOfRangeException(nameof(stockInStore), "There are no stores with the matching GUID");
            }

            laptops = laptops.Where(l => l.StoreLaptopStocks.Any(sls => sls.StoreId == stockInStore && sls.Quantity > 0)).ToList();

            CheckIfEmpty("No laptops have stock in the specified store");
        }
        else if (stockInProvince.HasValue)
        {
            if (!Enum.TryParse(typeof(Province), stockInProvince.ToString(), out object? enumValue))
            {
                throw new ArgumentOutOfRangeException(nameof(stockInProvince), "There are no provinces that match the input");
            }

            laptops = laptops.Where(l => l.StoreLaptopStocks.Any(sls => sls.Store.Province == stockInProvince && sls.Quantity > 0)).ToList();

            CheckIfEmpty("No laptops have stock in the specified province");
        }

        if (condition.HasValue)
        {
            if (!Enum.TryParse(typeof(LaptopCondition), condition.ToString(), out object? enumValue))
            {
                throw new ArgumentOutOfRangeException(nameof(condition), "There are no condition types that match the input");
            }

            laptops = laptops.Where(l => l.Condition == condition).ToList();

            CheckIfEmpty("No laptops fit the specified condition");
        }

        if (brandId.HasValue)
        {
            if (!db.Brands.Any(b => b.Id == brandId))
            {
                throw new ArgumentOutOfRangeException(nameof(stockInStore), "There are no brands with the matching GUID");
            }

            laptops = laptops.Where(l => l.BrandId == brandId).ToList();

            CheckIfEmpty("No laptops belong to the specified brand");
        }

        if (!string.IsNullOrEmpty(searchPhrase))
        {
            laptops = laptops.Where(l => l.Model.ToLower().Contains(searchPhrase.ToLower())).ToList();

            CheckIfEmpty("No laptops models match the search phrase");
        }

        return Results.Ok(laptops);

        void CheckIfEmpty(string message)
        {
            if (laptops.Count == 0) { throw new Exception(message); }
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/stores/{storeNumber}/inventory", (WebAppDBContext db, Guid storeNumber) =>
{
    try
    {
        List<Laptop> laptops;

        if (!db.Stores.Any(s => s.StoreNumber == storeNumber))
        {
            throw new ArgumentOutOfRangeException(nameof(storeNumber), "There are no stores with the matching GUID");
        }

        laptops = db.Laptops.Where(l => l.StoreLaptopStocks.Any(sls => sls.StoreId == storeNumber && sls.Quantity > 0)).ToList();

        if (laptops.Count == 0) { throw new Exception("No laptops have stock in the specified store"); }

        return Results.Ok(laptops);
    }
     catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/stores/{storeNumber}/{laptopNumber}/changeQuantity", (WebAppDBContext db, Guid storeNumber, Guid laptopNumber, int newQuantity) =>
{
    try
    {
        StoreLaptopStock? sls = db.StoreLaptopStocks.FirstOrDefault(sls => sls.StoreId == storeNumber && sls.LaptopId == laptopNumber);

        if (sls == null)
        {
            throw new ArgumentOutOfRangeException(message: "There is no stock relation between storeNumber and laptopNumber", innerException: null);
        }

        sls.Quantity = newQuantity;
        db.SaveChanges();

        return Results.Accepted($"/stores/{storeNumber}/{laptopNumber}", sls);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/brands/{brandNumber}/laptops/averagePrice", (WebAppDBContext db, Guid brandNumber) =>
{
    try
    {
        List<Laptop> laptops = db.Laptops.Where(l => l.BrandId == brandNumber).ToList();

        int laptopCount = laptops.Count;
        decimal averagePrice = laptops.Average(l => l.Price);

        object result = new
        {
            LaptopCount = laptopCount,
            AveragePrice = averagePrice,
        };

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/stores/groupByProvince", (WebAppDBContext db) =>
{
    try
    {
        List<StoreResult> storesByProvince = db.Stores
            .GroupBy(s => s.Province)
            .Where(group => group.Any())
            .Select(group => new StoreResult
            {
                Province = group.Key.ToString(),
                Stores = group.ToList()
            }).ToList();

        if (!storesByProvince.AsQueryable().Any())
        {
            throw new InvalidOperationException("No stores found in any province.");
        }

        return Results.Ok(storesByProvince);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

// only used for convenience on last endpoint
class StoreResult
{
    public string Province { get; set; }
    public List<Store> Stores { get; set; }
}