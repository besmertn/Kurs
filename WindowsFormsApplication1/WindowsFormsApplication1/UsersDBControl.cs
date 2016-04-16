using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    class UsersDBControl : DBControl
    {
        protected MD5 md5Hash = MD5.Create();
      
        private void ubdateUsersDB()
        {
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
                    hash = s.Substring(inx + 1, len - inx - 2);
                    string query = "INSERT INTO `Kurs`.`users` (`user_id`, `login`, `password`) VALUES (NULL,'" + login + "', '" + hash + "')";
                    this.Query(query);
                }
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        protected static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void addUser(string login, string password)
        {
            string path = @"D:\Git\WindowsFormsApplication1\usersDB.txt";

            string hash = "";
            using (md5Hash)
            {
                hash = GetMd5Hash(md5Hash, password);
            }
            string result = login + "*" + hash + ";";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(result);
            }

        }
        public Boolean authorizeCheck(string login, string password)
        {
            MySqlDataReader reader = readFrom("users");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //return reader.GetValue(1).ToString() + reader.GetString(2);
                    if (reader.GetValue(1).ToString() == login && VerifyMd5Hash(md5Hash, password, reader.GetString(2))) return true;
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            return false;
        }
        
    }
}
