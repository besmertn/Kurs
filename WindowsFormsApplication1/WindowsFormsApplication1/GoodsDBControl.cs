using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace WindowsFormsApplication1
{
    class GoodsDBControl : DBControl
    {
        Dictionary<string, Goods> goods = new Dictionary<string, Goods>();
        CultureInfo clt = new CultureInfo("ja-JP");
        protected void fillingList(string from)
        {
            MySqlDataReader reader = new DBControl().readFrom(from);
            while (reader.Read()) {
                goods.Add(reader.GetInt32("barcode").ToString(), new Goods(reader.GetInt32("barcode").ToString()
                    , reader.GetString("name")
                    , reader.GetString("measure")
                    , reader.GetInt32("count")
                    , reader.GetFloat("price")
                    , reader.GetDateTime("lastdelivery")
                    , reader.GetDateTime("shelflife")));
            }
        }

        public Goods searchGoods(string requiredBarcode) {
            return goods[requiredBarcode];
        }

        public void buyGoods(List<Goods> purchase) { 
            foreach(Goods product in purchase){
                goods[product.Barcode] -= product;
                Query("UPDATE `goods` SET `count` =  `count`-" + product.Count + " WHERE `barcode`= " + product.Barcode + " ;");
            }
        }
        public List<Goods> shelfLifeControl() {
            Dictionary<string, Goods> tmpGoodsDictionary = goods.Where(x => x.Value.ShelfLife.CompareTo(DateTime.Now) <= 0).ToDictionary(key => key.Key , value => value.Value);
            List<Goods> pastDueGoods = tmpGoodsDictionary.Values.ToList();
            foreach(Goods product in pastDueGoods){
                try
                {
                    Query("DELETE FROM `Kurs`.`goods` WHERE `goods`.`barcode` = " + product.Barcode + ";");
                    goods.Remove(product.Barcode);
                    Query("INSERT INTO `stitchedgoods`(`barcode`, `name`, `measure`, `count`, `price`, `shelflife`) VALUES(" + product.Barcode + ", '" + product.Name + "', '" + product.Measure + "', '" + product.Count + "', '" + product.Price + "', '" + product.ShelfLife.ToString("d", clt) + "')");
                }
                catch (MySqlException e) {
                    if (e.ErrorCode == 1062)
                    {
                        Query("INSERT INTO `stitchedgoods`(`barcode`, `name`, `measure`, `count`, `price`, `shelflife`) VALUES(NULL, '" + product.Name + "', '" + product.Measure + "', '" + product.Count + "', '" + product.Price + "', '" + product.ShelfLife.ToString("d", clt) + "')");
                    }
                }
            }
            return pastDueGoods;
        }

        public Boolean newDelivery(List<Goods> delivery) {
            foreach (Goods product in delivery) {
                
                try
                {
                    Query("INSERT INTO `goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES('" + product.Barcode + "', '" + product.Name + "', '" + product.Measure + "', '" + product.Count + "', '" + product.Price + "', '" + DateTime.Now.ToString("d", clt) + "', '" + product.ShelfLife.ToString("d", clt) + "')");
                    goods.Add(product.Barcode, new Goods(product.Barcode
                        , product.Name
                        , product.Measure
                        , product.Count
                        , product.Price
                        , product.LastDelivery
                        , product.ShelfLife));
                }
                catch (MySqlException e)
                {
                    if (e.ErrorCode == 1062)
                    {
                        Query("INSERT INTO `Kurs`.`goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES(NULL, '" +
                            product.Name + "', '" + product.Measure + "'), '" +
                            product.Count + "', '" + product.Price + "', '" +
                            DateTime.Now.ToString("d", clt) + "', '" +
                            product.ShelfLife.ToString("d", clt) + "'");
                        goods.Clear();
                        fillingList("goods");
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, Goods> createGoodsList() {
            fillingList("goods");
            return goods;
        }
        
    }
}
