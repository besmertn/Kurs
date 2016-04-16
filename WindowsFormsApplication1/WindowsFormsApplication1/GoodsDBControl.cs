﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    class GoodsDBControl : DBControl
    {
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
    }
}
