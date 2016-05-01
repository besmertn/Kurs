using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    class DBControl
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlDataReader reader;
        private void connect() {
            connection = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=Kurs");           
        }
        protected MySqlCommand Query(string query)
        {
            connect();
            connection.Open();
            command = new MySqlCommand(query,connection);
            command.ExecuteNonQuery();
            return command;
            
        }

        public MySqlDataReader readFrom(string tableName)
        {
            reader = Query("SELECT * FROM `" + tableName + "`").ExecuteReader();
            return reader;
        }


    }
}
