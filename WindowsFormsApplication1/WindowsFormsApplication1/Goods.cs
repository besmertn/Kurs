using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Goods
    {
        public string Barcode { set; get;}
        public string Name { set; get; }
        public string Measure { set; get; }
        public int Count { set; get; }
        public double Price { set; get; }
        public DateTime LastDelivery { set; get; }
        public DateTime ShelfLife{ set; get; }
        public Goods(string Barcode, string Name, string Measure, int Count, double Price, DateTime LastDelivery, DateTime ShelfLife) {
            this.Barcode = Barcode;
            this.Name = Name;
            this.Measure = Measure;
            this.Count = Count;
            this.Price = Price;
            this.LastDelivery = LastDelivery;
            this.ShelfLife = ShelfLife;
        }
        public Goods(string Barcode, string Name, string Measure, int Count, double Price)
        {
            this.Barcode = Barcode;
            this.Name = Name;
            this.Measure = Measure;
            this.Count = Count;
            this.Price = Price;
        }

        static public Goods operator -(Goods A, Goods B){
            A.Count -= B.Count;
            return A;
        }


    }
}
