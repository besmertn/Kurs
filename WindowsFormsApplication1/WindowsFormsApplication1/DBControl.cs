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

        protected MySqlDataReader readFrom(string tableName)
        {
            reader = Query("SELECT * FROM `" + tableName + "`").ExecuteReader();
            return reader;
        }
        public void updateUsersDB(){
            string path = @"D:\Git\WindowsFormsApplication1\usersDB.txt";
 
             using (StreamReader sr = File.OpenText(path))
             {
                 string s = "";
                 while ((s = sr.ReadLine()) != null)
                 {
                     string login = "";
                     string hash = "";
                     int inx = s.IndexOf("*");
                     int len = s.Length;
                     login = s.Substring(0, inx);
                     hash = s.Substring(inx + 1, len - inx-2);
                     try
                     {
                         string query = "INSERT INTO `Kurs`.`users` (`user_id`, `login`, `password`) VALUES (NULL,'" + login + "', '" + hash + "')";
                         this.Query(query);
                     }
                     catch {
                         string query = "UPDATE `Kurs`.`users` SET `password`='"+hash+"' WHERE `login` = '"+login+"')";
                         this.Query(query);
                     }


                     
                 }
             }
         }


    }
}
