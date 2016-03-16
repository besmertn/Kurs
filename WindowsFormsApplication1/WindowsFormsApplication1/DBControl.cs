using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace WindowsFormsApplication1
{
    class DBControl
    {
        public SQLiteDataAdapter main()
        {
            const string databaseName = @"D:\Git\WindowsFormsApplication1\base.db";
            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", databaseName));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM example", connection);
            //SQLiteDataReader dr = command.ExecuteReader();
            SQLiteDataAdapter sqlda = new SQLiteDataAdapter(command);
            return sqlda;
            
            
        }
    }
}
