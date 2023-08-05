namespace WebApplication2.Models
{
    public class StoreLaptopStock
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public Guid LaptopId { get; set; }
        public Laptop Laptop { get; set; }
        public StoreLaptopStock() { }
        public StoreLaptopStock(int quantity, Store store, Laptop laptop)
        {
            Quantity = quantity;
            StoreId = store.StoreNumber;
            Store = store;
            LaptopId = laptop.Number;
            Laptop = laptop;

            store.StoreLaptopStocks.Add(this);
            laptop.StoreLaptopStocks.Add(this);
        }
    }
}
