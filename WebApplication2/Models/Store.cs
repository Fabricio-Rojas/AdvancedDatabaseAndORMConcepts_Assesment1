namespace WebApplication2.Models
{
    public class Store
    {
        public Guid StoreNumber { get; set; }

        private string _address;
        public string Address // the street name and address
        {
            get => _address;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Store address must be at least three characters in length.");
                }
                _address = value;
            }
        } 

        public Province Province { get; set; }

        public HashSet<StoreLaptopStock> StoreLaptopStocks { get; set; } = new HashSet<StoreLaptopStock>();

        public Store() { }
        public Store(string address, Province province)
        {
            Address = address;
            Province = province;
        }
    }
    public enum Province
    {
        NL,
        PE,
        NS,
        NB,
        QC,
        ON,
        MB,
        SK,
        AB,
        BC,
        YT,
        NT,
        NU
    }
}
