using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Database
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            WebAppDBContext db = new WebAppDBContext(serviceProvider.GetRequiredService<DbContextOptions<WebAppDBContext>>());

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // BRAND
            Brand brandOne = new Brand("Asus");
            Brand brandTwo = new Brand("Acer");
            Brand brandThree = new Brand("Dell");

            if (!db.Brands.Any())
            {
                db.Brands.Add(brandOne);
                db.Brands.Add(brandTwo);
                db.Brands.Add(brandThree);
                db.SaveChanges();
            }

            // STORE
            Store storeOne = new Store("123 Fake St.", Province.MB);
            Store storeTwo = new Store("456 Rand Ave.", Province.AB);
            Store storeThree = new Store("789 None Blv.", Province.QC);

            if (!db.Stores.Any())
            {
                db.Stores.Add(storeOne);
                db.Stores.Add(storeTwo);
                db.Stores.Add(storeThree);
                db.SaveChanges();
            }

            // LAPTOP
            Laptop laptopOne = new Laptop("ZenBook 10", 999, LaptopCondition.New, brandOne);
            Laptop laptopTwo = new Laptop("VivoBook 9", 699, LaptopCondition.Rental, brandOne);
            Laptop laptopThree = new Laptop("Nitro 5", 1399, LaptopCondition.Refurbished, brandTwo);
            Laptop laptopFour = new Laptop("Swift 14", 1899, LaptopCondition.New, brandTwo);
            Laptop laptopFive = new Laptop("Inspiron 15", 649, LaptopCondition.Rental, brandThree);
            Laptop laptopSix = new Laptop("XPS 13", 1199, LaptopCondition.Refurbished, brandThree);

            if (!db.Laptops.Any())
            {
                db.Laptops.Add(laptopOne);
                db.Laptops.Add(laptopTwo);
                db.Laptops.Add(laptopThree);
                db.Laptops.Add(laptopFour);
                db.Laptops.Add(laptopFive);
                db.Laptops.Add(laptopSix);
                db.SaveChanges();
            }

            // STORELAPTOPSTOCK
            StoreLaptopStock slsOne = new StoreLaptopStock(-3, storeOne, laptopOne);
            StoreLaptopStock slsTwo = new StoreLaptopStock(5, storeOne, laptopTwo);
            StoreLaptopStock slsThree = new StoreLaptopStock(-1, storeOne, laptopThree);
            StoreLaptopStock slsFour = new StoreLaptopStock(0, storeOne, laptopFour);
            StoreLaptopStock slsFive = new StoreLaptopStock(15, storeTwo, laptopFive);
            StoreLaptopStock slsSix = new StoreLaptopStock(-10, storeTwo, laptopSix);
            StoreLaptopStock slsSeven = new StoreLaptopStock(7, storeTwo, laptopOne);
            StoreLaptopStock slsEight = new StoreLaptopStock(-6, storeTwo, laptopTwo);
            StoreLaptopStock slsNine = new StoreLaptopStock(26, storeThree, laptopThree);
            StoreLaptopStock slsTen = new StoreLaptopStock(-19, storeThree, laptopFour);
            StoreLaptopStock slsEleven = new StoreLaptopStock(37, storeThree, laptopFive);
            StoreLaptopStock slsTwelve = new StoreLaptopStock(-24, storeThree, laptopSix);

            if (!db.StoreLaptopStocks.Any())
            {
                db.StoreLaptopStocks.Add(slsOne);
                db.StoreLaptopStocks.Add(slsTwo);
                db.StoreLaptopStocks.Add(slsThree);
                db.StoreLaptopStocks.Add(slsFour);
                db.StoreLaptopStocks.Add(slsFive);
                db.StoreLaptopStocks.Add(slsSix);
                db.StoreLaptopStocks.Add(slsSeven);
                db.StoreLaptopStocks.Add(slsEight);
                db.StoreLaptopStocks.Add(slsNine);
                db.StoreLaptopStocks.Add(slsTen);
                db.StoreLaptopStocks.Add(slsEleven);
                db.StoreLaptopStocks.Add(slsTwelve);
                db.SaveChanges();
            }
        }
    }
}
