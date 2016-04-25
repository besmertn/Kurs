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
        private string[] pastDueGoodsArr;
        CultureInfo clt = new CultureInfo("ja-JP");
        public string[] goodsSearch(string barcode) {
            string[] result = new string[4];
            MySqlDataReader reader = readFrom("goods");
            while (reader.Read()) { 
                if(reader.GetValue(0).ToString().Equals(barcode)){
                    result[0] = reader.GetValue(0).ToString();
                    result[1] = reader.GetValue(1).ToString();
                    result[2] = reader.GetValue(2).ToString();
                    result[3] = reader.GetValue(4).ToString();
                }
            }
            return result;
        }

        public void buyUpdate(string[,] data, int length) {
            string query = "";
            for (int i = 0; i < length; i++) {
                query = "UPDATE `goods` SET `count` = `count`-" + data[i, 3] + " WHERE `barcode`=" + data[i, 0];
                Query(query);
            }
        }

        public string[] shelflifeControl() {
            MySqlDataReader reader = readFrom("goods");
            int pastDueGoodsCounter = 0;
            string query = "";
            while (reader.Read())
            {
               if(reader.GetDateTime("shelflife").CompareTo(DateTime.Now) <= 0)pastDueGoodsCounter++;
            }
            pastDueGoodsArr = new string[pastDueGoodsCounter];
            pastDueGoodsCounter = 0;
            reader = readFrom("goods");
            while (reader.Read())
            {
                if (reader.GetDateTime("shelflife").CompareTo(DateTime.Now) <= 0) pastDueGoodsArr[pastDueGoodsCounter++] = reader.GetValue(0).ToString();
            }
            reader = readFrom("goods");
            int i = 0;
            while (reader.Read())
            {
                if (reader.GetValue(0).ToString().Equals(pastDueGoodsArr[i]))
                {
                    try
                    {
                        query = "INSERT INTO `Kurs`.`stitchedgoods` (`barcode`, `name`, `measure`, `count`, `price`, `shelflife`) VALUES (" + reader.GetValue(0).ToString() + ", '" + reader.GetValue(1).ToString() + "', '" + reader.GetValue(2).ToString() + "', '" + reader.GetValue(3).ToString() + "', '" + reader.GetValue(4).ToString() + "', '" + reader.GetDateTime("shelflife").ToString("d",clt) + "')";
                        Query(query);
                        query = "DELETE FROM `Kurs`.`goods` WHERE `goods`.`barcode` = " + pastDueGoodsArr[i] + ";";
                        Query(query);
                    }
                    catch { 
                        
                    }
                    i++;
                    if (i == pastDueGoodsArr.Length) break;

                }
            }
            return pastDueGoodsArr;
        }

        public void newDelivery(string[,] data , int length) {
            string query = "";
            for (int i = 0; i < length; i++) {
                /*query = "INSERT INTO `Kurs`.`goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES (" + data[i, 0] + ", '" + data[i, 1] + "', '" + data[i, 2] + "', '" + data[i, 3] + "', '" + data[i, 4] + "', '" + DateTime.Now + "', '" + data[i, 5] + "')";
                if (QueryBool(query) == 0) { 
                    query = "UPDATE `Kurs`.`goods` SET `count` = '" + data[i, 3] + "', `price` = '" + data[i, 4] + "' ,`lastdelivery='"+ DateTime.Now +"'`, `shelflife`= '" + data[i, 5] + "' WHERE `goods`.`barcode` = " + data[i, 0];
                    Query(query);
                }*/

                try
                {
                    query = "INSERT INTO `Kurs`.`goods` (`barcode`, `name`, `measure`, `count`, `price`, `lastdelivery`, `shelflife`) VALUES (" + data[i, 0] + ", '" + data[i, 1] + "', '" + data[i, 2] + "', '" + data[i, 3] + "', '" + data[i, 4] + "', '" + DateTime.Now.ToString("d", clt) + "', '" + Convert.ToDateTime(data[i, 5]).ToString("d", clt) + "')";
                    Query(query);
                }
                catch {
                    query = "UPDATE `Kurs`.`goods` SET `count` = '" + data[i, 3] + "', `price` = '" + data[i, 4] + "' ,`lastdelivery`='" + DateTime.Now.ToString("d", clt) + "', `shelflife`= '" + Convert.ToDateTime(data[i, 5]).ToString("d", clt) + "' WHERE `goods`.`barcode` = " + data[i, 0];
                    Query(query);
                }/*CultureInfo.CreateSpecificCulture("ja-JP")*/
            }
        }
    }
}
