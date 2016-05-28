using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1.Controller
{
    internal class StitchedGoodsDbControl : GoodsDbControl
    {
        public void RemoveStitchedGoods(List<string> barcodes) {
            foreach (var barcode in barcodes) {
               Query("DELETE FROM `Kurs`.`stitchedgoods` WHERE `stitchedgoods`.`barcode` = " + barcode + ";");
            }
        }
        public List<Goods> GetAllStitchedGoods()
        {
            List<Goods> pastDueGoods = new List<Goods>();
            MySqlDataReader reader = ReadFrom("stitchedgoods");
            while (reader.Read()) {
                pastDueGoods.Add(new Goods(reader.GetInt32("barcode").ToString()
                    , reader.GetString("name")
                    , reader.GetString("measure")
                    , reader.GetInt32("count")
                    , reader.GetDouble("price")));
            }
            return pastDueGoods;
        }
    }
}
