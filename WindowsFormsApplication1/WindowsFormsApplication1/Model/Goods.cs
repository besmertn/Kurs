using System;

namespace WindowsFormsApplication1
{
    class Goods
    {
        public string Barcode { set; get; }
        public string Name { set; get; }
        public string Measure { set; get; }
        public int Count { set; get; }
        public double Price { set; get; }
        public DateTime LastDelivery { set; get; }
        public DateTime ShelfLife { set; get; }

        public Goods(string barcode, string name, string measure, int count, double price, DateTime lastDelivery,
            DateTime shelfLife)
        {
            Barcode = barcode;
            Name = name;
            Measure = measure;
            Count = count;
            Price = price;
            LastDelivery = lastDelivery;
            ShelfLife = shelfLife;
        }

        public Goods(string barcode, string name, string measure, int count, double price)
        {
            Barcode = barcode;
            Name = name;
            Measure = measure;
            Count = count;
            Price = price;
        }

        protected Goods(string barcode, string name)
        {
            Barcode = barcode;
            Name = name;
        }
    }
}
