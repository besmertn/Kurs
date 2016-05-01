using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    class StitchedGoodsDBControl : GoodsDBControl
    {
        List<Goods> pastDueGoods = new List<Goods>();

        public void RemoveStitchedGoods(List<string> barcodes) {
            foreach (string barcode in barcodes) {
               Query("DELETE FROM `Kurs`.`stitchedgoods` WHERE `stitchedgoods`.`barcode` = " + barcode + ";");
            }
        }
        public List< Goods> getAllStitchedGoods()
        {
            MySqlDataReader reader = readFrom("stitchedgoods");
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
