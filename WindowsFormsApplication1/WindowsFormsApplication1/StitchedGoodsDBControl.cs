using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    internal class StitchedGoodsDbControl : GoodsDbControl
    {
        readonly List<Goods> _pastDueGoods = new List<Goods>();

        public void RemoveStitchedGoods(List<string> barcodes) {
            foreach (var barcode in barcodes) {
               Query("DELETE FROM `Kurs`.`stitchedgoods` WHERE `stitchedgoods`.`barcode` = " + barcode + ";");
            }
        }
        public List< Goods> GetAllStitchedGoods()
        {
            MySqlDataReader reader = ReadFrom("stitchedgoods");
            while (reader.Read()) {
                _pastDueGoods.Add(new Goods(reader.GetInt32("barcode").ToString()
                    , reader.GetString("name")
                    , reader.GetString("measure")
                    , reader.GetInt32("count")
                    , reader.GetDouble("price")));
            }
            return _pastDueGoods;
        }
    }
}
