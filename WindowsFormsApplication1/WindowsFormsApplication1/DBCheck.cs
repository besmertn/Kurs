using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    class DBCheck : DBControl
    {
        public Boolean authorizeCheck(string login, string password){
            MySqlDataReader reader = readFrom("users");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //return reader.GetValue(1).ToString() + reader.GetString(2);
                    if (reader.GetValue(1).ToString() == login && VerifyMd5Hash(md5Hash , password, reader.GetString(2))) return true;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            return false;
        }
        private MySqlDataReader readFrom(string tableName) {
            MySqlDataReader reader = Query("SELECT * FROM `"+ tableName +"`").ExecuteReader();
            return reader;
        }
    }
}
